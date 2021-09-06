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
        Debug.Log("Beginning split task with index " + outputIndex);

        int indexHolder = 0;
        if (outputIndex != maxIndex - 1)
            indexHolder = outputIndex + 1;

        Debug.Log(indexHolder + " | " + maxIndex);

        while (indexHolder != outputIndex)
        {
            IOClass output = outputs[outputIndex];
            if (output.bin == null)
            {
                inputs[0].reserved = false;
                entity.transform.position = transform.position;
                entity.MoveTo(ResearchHandler.conveyorSpeed, output, this, true);
            }

            if (indexHolder != maxIndex - 1)
                indexHolder += 1;
            else indexHolder = 0;
        }

        outputIndex += 1;
        if (outputIndex == maxIndex - 1)
            outputIndex = 0;
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
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0], this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        inputs[0].bin = entity;
        SplitEntity(entity);
    }

    public override void OutputEntity(Entity entity)
    {
        
    }
}
