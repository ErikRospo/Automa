using UnityEngine;
using System.Collections.Generic;

// Buildings script
//
// This is the parent script of all buildings. A lot of this functionality
// has to do with conveyors. You can define buildings to have multiple inputs
// and outputs, as well as which tiles they should check for adjacent buildings.

public abstract class Building : BaseObject
{
    // Building data
    public BuildingData data;

    // Next / previous targets
    public IOClass[] inputs;
    public IOClass[] outputs;

    // Flag to determine if the building should be accepting entites
    public bool acceptingEntities = false;

    // Flag to tell the system the transforms on the object have been recycled.
    private bool positionsSet = false;

    // Hold the cells this building occupies
    [HideInInspector] public List<Vector2Int> cells;

    // Called by building handler if additional options should be applied
    public virtual void ApplyOptions(int option)
    {
        Debug.Log("This building does not have optional values!");
    }

    // Called by another building to input an entity
    public virtual bool InputEntity(ItemEntity entity)
    {
        Debug.Log("This building cannot input entities!");
        return false;
    }

    // Called once an entity gets to the input position
    public virtual void ReceiveEntity(ItemEntity entity)
    {
        Debug.Log("This building cannot receive entities!");
    }

    // Called once an entity is ready to be output
    public virtual void OutputEntity(ItemEntity entity)
    {
        Debug.Log("This building cannot output entities!");
    }

    // Used to update the bins on a building
    public virtual void UpdateBins()
    {
        Debug.Log("This building does not contain bins to update");
    }

    // Checks for nearby buildings
    public void CheckNearbyBuildings()
    {
        Debug.Log("Checking nearby buildings");

        // Loop through each input
        for (int i = 0; i < inputs.Length; i++)
        {
            Building building = BuildingHandler.active.TryGetBuilding(inputs[i].binTarget);
            if (building != null)
            {
                // Returns the index of the corresponding output
                int index = CheckInputPosition(building.outputs, i);

                if (index != -1)
                {
                    inputs[i].building = building;
                    inputs[i].target = building.outputs[index];
                    building.outputs[index].building = this;
                    building.outputs[index].target = inputs[i];
                    building.UpdateBins();
                }
                else
                {
                    Conveyor conveyor = building.GetComponent<Conveyor>();
                    if (conveyor != null && conveyor.isCorner) conveyor.CornerCheck(this);
                } 
            }
        }

        // Loop through each output 
        for (int i = 0; i < outputs.Length; i++)
        {
            Building building = BuildingHandler.active.TryGetBuilding(outputs[i].binTarget);

            if (building != null)
            {
                // Returns the index of the corresponding input
                int index = CheckOutputPosition(building.inputs, i);

                if (index != -1)
                {
                    outputs[i].building = building;
                    outputs[i].target = building.inputs[index];
                    building.inputs[index].building = this;
                    building.inputs[index].target = outputs[i];
                    UpdateBins();
                }
            }
        }
    }

    // Must be called by each building
    // 
    // This method recycles all transforms on an object to alleviate memory / transform updates
    // by the engine. The object then holds a reference to those tiles via a Vector3. This saves
    // on the fly calculations in the future, and is a lot easier to setup and manage.
    public void SetupPositions()
    {
        if (positionsSet) return;

        // Setup input positions
        for (int i = 0; i < inputs.Length; i++)
            inputs[i].SetupPosition(transform.position, transform.rotation.eulerAngles.z);

        // Setup output positions
        for (int i = 0; i < outputs.Length; i++)
            outputs[i].SetupPosition(transform.position, transform.rotation.eulerAngles.z);

        positionsSet = true;
    }

    // Checks if building is in an input position
    public int CheckInputPosition(IOClass[] outputs, int index)
    {
        for (int a = 0; a < outputs.Length; a++)
            if (new Vector2(inputs[index].binConnector.x, inputs[index].binConnector.y) == outputs[a].binPosition)
                return a;
        return -1;
    }

    // Checks if building is in an output position
    public int CheckOutputPosition(IOClass[] inputs, int index)
    {
        for (int a = 0; a < inputs.Length; a++)
            if (new Vector2(outputs[index].binConnector.x, outputs[index].binConnector.y) == inputs[a].binPosition)
                return a;
        return -1;
    }

    // Destroys the entity
    public override void Destroy()
    {
        // Iterate through inputs and remove any entities
        foreach (IOClass input in inputs)
            if (input.bin != null)
                Recycler.AddRecyclable(input.bin.transform);

        // Iterate through outputs and remove any entities
        foreach (IOClass output in outputs)
            if (output.bin != null)
                Recycler.AddRecyclable(output.bin.transform);

        // Destroy gameObject
        Destroy(gameObject);
    }
}
