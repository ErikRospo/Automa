using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Containers
    public Entity frontBin;
    public Entity rearBin;
    public Transform frontPos;
    public Transform rearPos;

    // Hold a reference to the previous belt
    public Building nextTarget;
    public Conveyor previousConveyor;
    
    // Speed of the conveyor (temp)
    public float speed;

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

    private void SetRotation()
    {
        // Set rotation
        switch (transform.rotation.eulerAngles.z)
        {
            case 90f:
                Debug.Log("Checking top tile");
                frontTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rearTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rotation = rotationType.NORTH;
                break;
            case 180f:
                Debug.Log("Checking left tile");
                frontTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rotation = rotationType.WEST;
                break;
            case 270f:
                Debug.Log("Checking bottom tile");
                frontTile = new Vector2(transform.position.x, transform.position.y - 5f);
                rearTile = new Vector2(transform.position.x, transform.position.y + 5f);
                rotation = rotationType.SOUTH;
                break;
            default:
                Debug.Log("Checking right tile");
                frontTile = new Vector2(transform.position.x + 5f, transform.position.y);
                rearTile = new Vector2(transform.position.x - 5f, transform.position.y);
                rotation = rotationType.EAST;
                break;
        }
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
            if (building != null) nextTarget = building;
        }

        conveyor = BuildingHandler.TryGetConveyor(rearTile);
        if (conveyor != null && conveyor.rotation == rotation)
        {
            conveyor.nextTarget = this;
            conveyor.UpdateBin();
            previousConveyor = conveyor;
        }
    }

    public override void PassEntity(Entity entity)
    {
        entity.SetConveyorTarget(speed, rearPos.position, this);
    }

}
