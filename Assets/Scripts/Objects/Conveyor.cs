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
    public Transform frontPos;
    public Transform rearPos;
    private bool frontReserved;

    // Hold a reference to the previous belt
    [HideInInspector] public Building nextTarget;
    [HideInInspector] public Conveyor previousConveyor;
    
    // Speed of the conveyor (temp)
    public float speed;
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
        if (entity.transform.position == rearPos.position)
        {
            rearBin = entity;
            acceptingEntities = false;
        }
        else frontBin = entity;
        UpdateBin();
    }

    public void ToggleCorner()
    {
        frontPos.localPosition = new Vector2(0, frontPos.localPosition.x);

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
            nextTarget.PassEntity(frontBin);
            frontBin = null;
            frontReserved = false;
        }
        
        // Check the back container
        if (rearBin != null && frontBin == null && !frontReserved)
        {
            rearBin.SetConveyorTarget(speed, frontPos.position, this);
            rearBin = null;
            acceptingEntities = true;
            frontReserved = true;

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

    public override void PassEntity(Entity entity)
    {
        acceptingEntities = false;
        entity.SetConveyorTarget(speed, rearPos.position, this);
    }
}
