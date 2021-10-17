using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Building
{
    public void Start()
    {
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Set the input target
    public override bool SetInputTarget(Building target)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (target.transform.position == inputs[i].tilePosition)
            {
                inputs[i].target = target;
                return true;
            }
        }
        return false;
    }

    // Called when an entity is ready to be sent 
    public override bool InputEntity(Entity entity)
    {
        entity.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
        return true;
    }

    // Called when entity arrives at input bin
    public override void ReceiveEntity(Entity entity)
    {
        Recycler.AddRecyclable(entity.transform);
    }
}
