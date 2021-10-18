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
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Update bins method (updates input / output bins)
    public override void UpdateBins()
    {
        for (int i = 0; i < 3; i++)
        {
            if (outputs[i].bin != null && // If output bin is not empty
                outputs[i].target != null && // If target is not null
                outputs[i].target.acceptingEntities && // If target is accepting entities
                outputs[i].target.InputEntity(outputs[i].bin)) // If target is able to take entity
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
                inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[output].position, this, true);
                
                // Open the input position
                inputs[0].bin = null;
                acceptingEntities = true;

                // Update input target if one exists
                if (inputs[0].target != null)
                    inputs[0].target.UpdateBins();

                break;
            }
        }
    }
    

    // Input method (called from output buildings)
    public override bool InputEntity(Entity entity)
    {
        acceptingEntities = false;
        inputs[0].reserved = true;
        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].position, this);
        return true;
    }

    // Receive method (called when entity arrives at splitter)
    public override void ReceiveEntity(Entity entity)
    {
        inputs[0].bin = entity;
        inputs[0].reserved = false;
        SplitInput();
    }

    // Output method (called when entity arrives at an output
    public override void OutputEntity(Entity entity)
    {
        outputs[entity.outputIndex].bin = entity;
        outputs[entity.outputIndex].reserved = false;
        UpdateBins();
    }

    // Sets the input target
    public override bool SetInputTarget(Building target)
    {
        inputs[0].target = target;
        return true;
    }

    // Sets the output target 
    public override bool SetOutputTarget(Building target)
    {
        for (int i = 0; i < outputs.Length; i++)
        {
            if (target.transform.position == outputs[i].tilePosition)
            {
                outputs[i].target = target;
                UpdateBins();
                return true;
            }
        }
        return false;
    }

    // Checks for nearby buildings
    public override void CheckNearbyBuildings()
    {
        // Local building parameter;
        Building building;

        // Set input target
        building = BuildingHandler.active.TryGetBuilding(inputs[0].tilePosition);
        if (building != null && building.rotation == rotation)
            if (!building.SetOutputTarget(this)) SetInputTarget(building);

        // Loop through each output
        for (int i = 0; i < outputs.Length; i++)
        {
            building = BuildingHandler.active.TryGetBuilding(outputs[i].tilePosition);
            if (building != null)
            {
                foreach (IOClass input in building.inputs)
                {
                    if (input.tilePosition == transform.position)
                    {
                        if (!building.SetInputTarget(this)) break;
                        SetOutputTarget(building);
                    }
                }
            }
        }
    }
}
