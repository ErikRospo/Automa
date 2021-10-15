using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Building
{
    // Holds what output is currently set
    private enum Outputs
    {
        LEFT = 0,
        TOP = 1,
        RIGHT = 2
    }
    private Outputs output;

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
        Debug.Log("Updating bins");

        // Save starting spot
        Outputs start = output;

        // Move to next output
        if ((int)output == 2) output = 0;
        else output += 1;

        // THSI IS INFINTE LOOPA FACK

        // Iterate through all outputs
        while (output != start)
        {
            // Grab index of current position
            int index = (int)output;
            Debug.Log("Checking output: " + index);

            // Move to next output
            if ((int)output == 2) output = 0;
            else output += 1;

            // Attempt to output any entities
            if (outputs[index].bin != null)
            {
                if (outputs[index].target.InputEntity(outputs[index].bin))
                {
                    Debug.Log("Splitting output " + index + " and checking input");
                    outputs[index].bin = null;
                    outputs[index].reserved = false;
                    CheckInput(index);
                }
            }
            else
            {
                Debug.Log("Output " + index + " is empty, checking input");
                CheckInput(index);
            }
        }
    }
    
    public void CheckInput(int index)
    {
        // Check input bin
        if (inputs[0].bin != null)
        {
            inputs[0].bin.MoveTo(ResearchHandler.conveyorSpeed, outputs[index].position, this);
            inputs[0].bin = null;
            acceptingEntities = true;

            if (inputs[0].target != null)
                inputs[0].target.UpdateBins();
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
        acceptingEntities = false;
        UpdateBins();
    }


    // Sets the input target
    public override void SetInputTarget(Building target)
    {
        inputs[0].target = target;
    }

    // Sets the output target 
    public override void SetOutputTarget(Building target)
    {
        for (int i = 0; i < outputs.Length; i++)
            if (target.transform.position == outputs[i].tilePosition)
                outputs[i].target = target;
        UpdateBins();
    }
}
