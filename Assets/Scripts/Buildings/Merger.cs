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
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();
    }

    // Update bins method (updates input / output bins)
    public override void UpdateBins()
    {
        if (outputs[0].bin != null &&
            outputs[0].target != null &&
            outputs[0].target.acceptingEntities &&
            outputs[0].target.InputEntity(outputs[0].bin))
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
                inputs[i].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[0].position, this, true);
                inputs[i].bin = null;
                outputs[0].reserved = true;

                // Update input target
                if (inputs[i].target != null)
                    inputs[i].target.UpdateBins();

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
            if (entity.lastBuilding == inputs[i].target && 
                inputs[i].bin == null && !inputs[i].reserved)
            {
                // Save index and move to input position
                inputs[i].reserved = true;
                entity.outputIndex = i;
                entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[i].position, this);
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

    // Sets the input target
    public override bool SetInputTarget(Building target)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (target.transform.position == inputs[i].tilePosition)
            {
                inputs[i].target = target;
                inputs[i].target.UpdateBins();
                return true;
            }
        }
        return false;
    }

    // Sets the output target 
    public override bool SetOutputTarget(Building target)
    {
        outputs[0].target = target;
        UpdateBins();
        return true;
    }

    // Checks for nearby buildings
    public override void CheckNearbyBuildings()
    {
        // Local building parameter;
        Building building;

        // Loop through each input
        for (int i = 0; i < inputs.Length; i++)
        {
            building = BuildingHandler.active.TryGetBuilding(inputs[i].tilePosition);
            if (building != null)
            {
                foreach (IOClass output in building.outputs) 
                {
                    if (output.tilePosition == transform.position)
                    {
                        if (!building.SetOutputTarget(this)) break;
                        SetInputTarget(building);
                    }
                }
            }
        }

        // Loop through each output 
        building = BuildingHandler.active.TryGetBuilding(outputs[0].tilePosition);
        if (building != null && building.rotation == rotation)
            if (!building.SetInputTarget(this)) SetOutputTarget(building);
    }
}
