using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Animation
    private Animator animator;

    // Containers
    [HideInInspector] public Entity frontBin;
    [HideInInspector] public Entity rearBin;
    public Transform frontPos;
    public Transform rearPos;

    // Hold a reference to the previous belt
    [HideInInspector] public Building nextTarget;
    [HideInInspector] public Conveyor previousConveyor;
    
    // Speed of the conveyor (temp)
    public float speed;
    public bool isCorner;

    // Holds positions to the front and back tiles
    private Vector2 frontTile, rearTile, leftTile, rightTile;

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
        animator = GetComponent<Animator>();

        SetRotation();
        CheckNearbyConveyors();
    }

    // Sets a conveyor bin
    public void SetBin(Entity entity)
    {
        if (entity.transform.position == rearPos.position)
        {
            rearBin = entity;
            acceptingEntities = false;
        }
        else frontBin = entity;
        UpdateBin();
    }

    public void UpdateBin()
    {
        // Checks the front container
        if (frontBin != null && nextTarget != null && nextTarget.acceptingEntities)
        {
            nextTarget.PassEntity(frontBin);
            frontBin = null;
        }
        
        // Check the back container
        if (rearBin != null && frontBin == null)
        {
            rearBin.SetConveyorTarget(speed, frontPos.position, this);
            rearBin = null;
            acceptingEntities = true;

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
                leftTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rightTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rotation = rotationType.NORTH;
                break;
            case 180f:
                frontTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x + 5f, transform.position.y);
                leftTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rightTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rotation = rotationType.WEST;
                break;
            case 270f:
                frontTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rearTile = new Vector2(transform.position.x, transform.position.y + 5f);
                leftTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rightTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rotation = rotationType.SOUTH;
                break;
            default:
                frontTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x - 5f, transform.position.y);
                leftTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rightTile = new Vector2(transform.position.x, transform.position.y - 5f);
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
            else CornerCheck(conveyor);
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

        animator.Play(0, -1, AnimationHandler.conveyorMaster.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void CornerCheck(Conveyor conveyor)
    {
        // Check to make sure conveyor is not facing the same direction
        if (conveyor.rotation == rotationType.EAST  && rotation == rotationType.WEST  ||
            conveyor.rotation == rotationType.NORTH && rotation == rotationType.SOUTH ||
            conveyor.rotation == rotationType.WEST  && rotation == rotationType.EAST  ||
            conveyor.rotation == rotationType.SOUTH && rotation == rotationType.NORTH) return;

        // Check to make sure the conveyor does not have a previous conveyor target
        if (conveyor.previousConveyor != null) return;

        // Setup the rotation
    }

    public override void PassEntity(Entity entity)
    {
        entity.SetConveyorTarget(speed, rearPos.position, this);
    }
}
