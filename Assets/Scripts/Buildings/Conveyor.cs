using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Animation
    public Animator animator;

    // Containers
    [HideInInspector] public Entity frontBin;
    [HideInInspector] public Entity rearBin;

    // Hold a reference to the previous belt
    [HideInInspector] public Building nextTarget;
    [HideInInspector] public Conveyor previousConveyor;
    
    // Speed of the conveyor (temp)
    public bool isCorner;
    public float sizeAdjust;

    // Holds the rotation value for comparisons
    public enum rotationType
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
    public rotationType rotation;

    private void Start()
    {
        SetupPositions();
        transform.localScale = new Vector2(sizeAdjust, sizeAdjust);
        CheckNearbyConveyors();
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
        UpdateBin();
    }

    public void ToggleCorner()
    {
        outputPositions[0] = new Vector2(0, outputPositions[0].x);

        isCorner = true;
        animator.enabled = !animator.enabled;
        if (!animator.enabled)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buildings/ConveyorTurn");
    }

    public void UpdateBin()
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

            if (previousConveyor != null)
                previousConveyor.UpdateBin();
        }
    }

    public void CheckNearbyConveyors()
    {
        // Check the right position
        Conveyor conveyor = BuildingHandler.TryGetConveyor(outputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.rotation == rotation)
            {
                conveyor.previousConveyor = this;
                nextTarget = conveyor;
            }
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
            if (building != null)
                nextTarget = building;
        }

        conveyor = BuildingHandler.TryGetConveyor(inputTilePositions[0]);
        if (conveyor != null && conveyor.rotation == rotation)
        {
            conveyor.nextTarget = this;
            conveyor.UpdateBin();
            previousConveyor = conveyor;
        }
        else if (conveyor != null && conveyor.isCorner) CornerCheck(conveyor);

        animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void CornerCheck(Conveyor conveyor)
    {
        // Check to make sure conveyor is not facing the same direction
        if (conveyor.rotation == rotationType.NORTH && rotation == rotationType.WEST ||
            conveyor.rotation == rotationType.EAST && rotation == rotationType.NORTH ||
            conveyor.rotation == rotationType.SOUTH && rotation == rotationType.EAST ||
            conveyor.rotation == rotationType.WEST && rotation == rotationType.SOUTH)
        {
            conveyor.nextTarget = this;
            conveyor.UpdateBin();
            previousConveyor = conveyor;
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
