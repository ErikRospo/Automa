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

    public float speed;

    private void Start()
    {
        CheckNearbyConveyors();
    }

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
        Conveyor conveyor = BuildingHandler.TryGetConveyor(new Vector2(transform.position.x + 5f, transform.position.y));
        if (conveyor != null)
        {
            conveyor.previousConveyor = this;
            nextTarget = conveyor;
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(new Vector2(transform.position.x + 5f, transform.position.y));
            if (building != null) nextTarget = building;
        }

        conveyor = BuildingHandler.TryGetConveyor(new Vector2(transform.position.x - 5f, transform.position.y));
        if (conveyor != null)
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
