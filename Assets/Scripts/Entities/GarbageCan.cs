using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Building
{
    public void Start()
    {
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Called when an entity is ready to be sent 
    public override bool InputEntity(Item entity)
    {
        entity.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
        return true;
    }

    // Called when entity arrives at input bin
    public override void ReceiveEntity(Item entity)
    {
        Recycler.AddRecyclable(entity.transform);
    }
}
