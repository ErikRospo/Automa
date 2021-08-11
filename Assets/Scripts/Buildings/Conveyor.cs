using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Building
{
    // Containers
    private Entity frontContainer;
    private Entity backContainer;
    public Vector3 frontPosition;
    public Vector3 backPosition;
    public bool frontOccupied;
    public bool backOccupied;

    // Hold a reference to the previous belt
    public Building nextTarget;
    public Conveyor previousConveyor;

    public float speed;

    private void Start()
    {
        CheckNearbyConveyors();
    }

    public void updateContainer()
    {
        // Checks the front container
        if (frontContainer != null)
        {
            nextTarget.PassEntity(frontContainer);
            frontContainer = null;
            frontOccupied = false;
        }
        
        // Check the back container
        if (backContainer != null && !frontOccupied)
        {
            backContainer.SetTarget(speed, frontPosition);
            backContainer = null;
            backOccupied = false;
            frontOccupied = true;

            if (previousConveyor != null)
                previousConveyor.updateContainer();
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
            previousConveyor = conveyor;
        }
    }

    public override bool PassEntity(Entity entity)
    {
        entity.SetTarget(speed, backPosition);
        backOccupied = true;
        return true;
    }

}
