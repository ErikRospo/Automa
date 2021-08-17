using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Animation
    public Animator animator;
    
    // Speed of the conveyor (temp)
    public bool isCorner;
    public float sizeAdjust;

    private void Start()
    {
        SetupRotation();
        SetupPositions();
        transform.localScale = new Vector2(sizeAdjust, sizeAdjust);
        CheckNearbyBuildings();
    }

    // Togles enabling a corner conveyor
    public void ToggleCorner(bool rotateUp)
    {
        if (rotateUp) outputs[0].localPosition = new Vector2(0, outputs[0].localPosition.x);
        else outputs[0].localPosition = new Vector2(0, -outputs[0].localPosition.x);

        isCorner = true;
        animator.enabled = !animator.enabled;
        if (!animator.enabled)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buildings/ConveyorTurn");
    }

    // Sets a conveyor bin
    public void SetBin(Entity entity)
    {
        if (entity.transform.position == inputPositions[0])
        {
            rearBin = entity;
            acceptingEntities = false;
        }
        else frontBin = entity;
        UpdateBins();
    }

    public override void UpdateBins()
    {
        // Checks the front container
        if (frontBin != null && nextTarget != null && nextTarget.acceptingEntities)
        {
            if (nextTarget.PassEntity(frontBin))
            {
                frontBin = null;
                outputReserved = false;
            }
        }
        
        // Check the back container
        if (rearBin != null && frontBin == null && !outputReserved)
        {
            if (isCorner) rearBin.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
            else rearBin.MoveTo(ResearchHandler.conveyorSpeed, outputPositions[0], this);

            rearBin = null;
            acceptingEntities = true;
            outputReserved = true;
            inputReserved = false;

            if (previousTarget != null)
                previousTarget.UpdateBins();
        }
    }

    public void CheckNearbyBuildings()
    {
        // Check the front bin
        Conveyor conveyor = BuildingHandler.TryGetConveyor(outputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.rotation == rotation)
            {
                conveyor.previousTarget = this;
                nextTarget = conveyor;
            }
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
            if (building != null)
                nextTarget = building;
        }

        // Check the rear bin
        conveyor = BuildingHandler.TryGetConveyor(inputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.rotation == rotation)
            {
                conveyor.nextTarget = this;
                conveyor.UpdateBins();
                previousTarget = conveyor;
            }
            else if (conveyor.isCorner) CornerCheck(conveyor);
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(inputTilePositions[0]);
            if (building != null)
            {
                building.nextTarget = this;
                building.UpdateBins();
            }
        }

        animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    public void CornerCheck(Building building)
    {
        // Check to make sure conveyor is not facing the same direction
        if (building.rotation == rotationType.NORTH && rotation == rotationType.WEST ||
            building.rotation == rotationType.EAST && rotation == rotationType.NORTH ||
            building.rotation == rotationType.SOUTH && rotation == rotationType.EAST ||
            building.rotation == rotationType.WEST && rotation == rotationType.SOUTH)
        {
            building.nextTarget = this;
            building.UpdateBins();
            previousTarget = building;
        }
    }

    public override bool PassEntity(Entity entity)
    {
        acceptingEntities = false;
        inputReserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputPositions[0], this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputPositions[0], this);
        else SetBin(entity);
    }
}
