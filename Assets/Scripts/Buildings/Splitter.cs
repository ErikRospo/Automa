using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Building
{
    public Tile tile;
    public int outputIndex = 0;
    private int maxIndex;

    public void Start()
    {
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();

        maxIndex = outputs.Length;
    }

    public void SplitEntity(Entity entity)
    {
        Debug.Log(outputIndex);
        inputs[0].bin = entity;

        IOClass output = outputs[outputIndex];
        if (output.bin == null)
        {
            entity.transform.position = transform.position;
            EntityHandler.RegisterMovingEntity(ResearchHandler.conveyorSpeed, output.position, entity, this);
        }
    }

    public override void SetInputTarget(Building target)
    {
        inputs[0].target = target;
    }

    public override void SetOutputTarget(Building target)
    {
        for (int i = 0; i < outputs.Length; i++)
            if (target.transform.position == outputs[i].tilePosition)
                outputs[i].target = target;

        UpdateBins();
    }

    // Called when an entity is ready to be sent 
    public override bool InputEntity(Entity entity)
    {

        EntityHandler.RegisterMovingEntity(ResearchHandler.conveyorSpeed, inputs[0].position, entity, this);
        return true;

    }

    public override void ReceiveEntity(Entity entity)
    {
        SplitEntity(entity);
    }
}
