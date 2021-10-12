using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Building
{
    public int index = 0;
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
        // Set index
        int holder = index;
        index += 1;
        if (index == maxIndex)
            index = 0;

        while (index != holder)
        {
            IOClass output = outputs[index];
            if (output.bin == null && output.target != null)
            {
                MoveInput(inputs[0], output);
                entity.outputIndex = index;
                entity.MoveTo(ResearchHandler.conveyorSpeed, transform.position, this);
                break;
            }
            else
            {
                index += 1;
                if (index == maxIndex)
                    index = 0;
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

    public override void SetOutputTarget(IOClass output, Building target)
    {
        if (output != null)
        {
            output.target = target;
            UpdateBins();
        }
        else SetOutputTarget(target);
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
            acceptingEntities = false;
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
                MoveOutput(output);
                entity.outputIndex = -1;
            }
        }
        else
        {
            Debug.LogError("Issue while splitting entity");
            Recycler.AddRecyclable(entity.transform);
        }
    }
}
