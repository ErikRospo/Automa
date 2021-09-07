using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Building
{
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

        // Set index
        int holder = outputIndex;
        outputIndex += 1;
        if (outputIndex == maxIndex)
            outputIndex = 0;

        while (outputIndex != holder)
        {
            IOClass output = outputs[outputIndex];
            if (output.bin == null)
            {
                inputs[0].reserved = false;
                output.reserved = true;
                entity.outputIndex = outputIndex;
                entity.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
                break;
            }
            else
            {
                outputIndex += 1;
                if (outputIndex == maxIndex)
                    outputIndex = 0;
            }
        }
    }

    public override void UpdateBins()
    {
        foreach (IOClass holder in outputs)
            if (holder.bin != null)
                OutputEntity(holder.bin);
        if (inputs[0].bin != null) SplitEntity(inputs[0].bin);
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
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].position, this);
        return true;
    }

    public override void ReceiveEntity(Entity entity)
    {
        if (entity.transform.position == transform.position)
        {
            entity.MoveTo(ResearchHandler.conveyorSpeed, outputs[entity.outputIndex].position, this, true);
        }
        else
        {
            inputs[0].bin = entity;
            SplitEntity(entity);
        }
    }

    public override void OutputEntity(Entity entity)
    {
        IOClass output = outputs[entity.outputIndex];

        if (output != null)
        {
            output.bin = entity;
            if (output.target != null &&
                output.target.acceptingEntities &&
                output.target.InputEntity(entity))
            {
                output.bin = null;
                output.reserved = false;
            }
        }
    }
}
