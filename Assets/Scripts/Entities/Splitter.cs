using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Building
{
    // Holds what output is currently set
    private int output;

    // Setup the building
    public void Start()
    {
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Update bins method (updates input / output bins)
    public override void UpdateBins()
    {
        for (int i = 0; i < 3; i++)
        {
            if (outputs[i].bin != null && // If output bin is not empty
                outputs[i].building != null && // If target is not null
                outputs[i].building.acceptingEntities && // If target is accepting entities
                outputs[i].building.InputEntity(outputs[i].bin)) // If target is able to take entity
            {
                outputs[i].bin = null;
                SplitInput();
            }
        }
    }

    // Splits the input into one of the outputs 
    public void SplitInput()
    {
        // Check if there is an entity to split
        if (inputs[0].bin == null) return;

        // Iterate through all outputs
        for (int i = 0; i < 3; i++)
        {
            // Loop through output
            output += 1;
            if (output > 2) output = 0;

            // Check if output bin is available
            if (outputs[output].bin == null &&
                !outputs[output].reserved)
            {
                // Save reference to output index
                inputs[0].bin.outputIndex = output;

                // Reserve the output position
                outputs[output].reserved = true;

                // Move input to output position
                inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[output].binPosition, this, true);
                
                // Open the input position
                inputs[0].bin = null;
                acceptingEntities = true;

                // Update input target if one exists
                if (inputs[0].building != null)
                    inputs[0].building.UpdateBins();

                break;
            }
        }
    }
    

    // Input method (called from output buildings)
    public override bool InputEntity(Item entity)
    {
        acceptingEntities = false;
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].binPosition, this);
        return true;
    }

    // Receive method (called when entity arrives at splitter)
    public override void ReceiveEntity(Item entity)
    {
        inputs[0].bin = entity;
        inputs[0].reserved = false;
        SplitInput();
    }

    // Output method (called when entity arrives at an output
    public override void OutputEntity(Item entity)
    {
        outputs[entity.outputIndex].bin = entity;
        outputs[entity.outputIndex].reserved = false;
        UpdateBins();
    }
}
