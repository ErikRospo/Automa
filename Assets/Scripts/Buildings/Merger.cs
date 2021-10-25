using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : Building
{
    // Holds what output is currently set
    private int input;

    // Setup the building
    public void Start()
    {
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Update bins method (updates input / output bins)
    public override void UpdateBins()
    {
        if (outputs[0].bin != null &&
            outputs[0].building != null &&
            outputs[0].building.acceptingEntities &&
            outputs[0].building.InputEntity(outputs[0].bin))
        {
            outputs[0].bin = null;
            MergeOutput();
        }
    }

    // Merges the inputs into the output
    public void MergeOutput()
    {
        // Check if there is an entity to merge
        if (outputs[0].bin != null || outputs[0].reserved) return;

        // Iterate through all outputs
        for (int i = 0; i < 3; i++)
        {
            // Loop through output
            input += 1;
            if (input > 2) input = 0;

            // If the input is not null, move it to output
            if (inputs[i].bin != null)
            {
                // Clear bin and move to output
                inputs[i].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].binPosition, this, true);
                inputs[i].bin = null;
                outputs[0].reserved = true;

                // Update input target
                if (inputs[i].building != null)
                    inputs[i].building.UpdateBins();

                return;
            }
        }
    }

    // Input method (called from output buildings)
    public override bool InputEntity(Entity entity)
    {
        // Loops through inputs
        for(int i = 0; i < 3; i++)
        {
            // Grab entity at corresponding input
            if (entity.lastBuilding == inputs[i].building && 
                inputs[i].bin == null && !inputs[i].reserved)
            {
                // Save index and move to input position
                inputs[i].reserved = true;
                entity.outputIndex = i;
                entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[i].binPosition, this);
                return true;
            }
        }
        return false;
    }

    // Receive method (called when entity arrives at merger)
    public override void ReceiveEntity(Entity entity)
    {
        inputs[entity.outputIndex].bin = entity;
        inputs[entity.outputIndex].reserved = false;
        MergeOutput();
    }

    // Output method (called when entity arrives at an output)
    public override void OutputEntity(Entity entity)
    {
        outputs[0].bin = entity;
        outputs[0].reserved = false;
        UpdateBins();
    }
}
