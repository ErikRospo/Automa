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

    // Holds positions to the front and back tiles
    private Vector2 frontTile, rearTile;

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
        SetRotation();
        CheckNearbyConveyors();
    }

    // Sets a conveyor bin
    public void SetBin(Entity entity)
    {
        if (entity.transform.position == inputPositions[0].position)
        {
            rearBin = entity;
            acceptingEntities = false;
        }
        else frontBin = entity;
        UpdateBin();
    }

    public void ToggleCorner()
    {
        outputPositions[0].localPosition = new Vector2(0, outputPositions[0].localPosition.x);

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
            else rearBin.MoveTo(ResearchHandler.conveyorSpeed, outputPositions[0].position, this);

            rearBin = null;
            acceptingEntities = true;
            outputReserved = true;
            inputReserved = false;

            if (previousConveyor != null)
                previousConveyor.UpdateBin();
        }
    }

    private void SetRotation()
    {
        // Set rotation
        switch (transform.rotation.eulerAngles.z)
        {
            case 90f:
                frontTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rearTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rotation = rotationType.NORTH;
                break;
            case 180f:
                frontTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rotation = rotationType.WEST;
                break;
            case 270f:
                frontTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rearTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rotation = rotationType.SOUTH;
                break;
            default:
                frontTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rotation = rotationType.EAST;
                break;
        }
    }

    public void CheckNearbyConveyors()
    {
        // Check the right position
        Conveyor conveyor = BuildingHandler.TryGetConveyor(frontTile);
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
            Building building = BuildingHandler.TryGetBuilding(frontTile);
            if (building != null)
                nextTarget = building;
        }

        conveyor = BuildingHandler.TryGetConveyor(rearTile);
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
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputPositions[0].position, this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputPositions[0].position, this);
        else SetBin(entity);
    }
}
