using UnityEngine;
using System.Collections.Generic;
using Mirror;

// Buildings script
//
// This is the parent script of all buildings. A lot of this functionality
// has to do with conveyors. You can define buildings to have multiple inputs
// and outputs, as well as which tiles they should check for adjacent buildings.

public abstract class Building : NetworkBehaviour, IDamageable
{
    // Next / previous targets
    public IOClass[] inputs;
    public IOClass[] outputs;

    // Flag to determine if the building should be accepting entites
    public bool acceptingEntities = false;
    public bool skipInputCheck = false;
    public bool skipOutputCheck = false;

    // Flag to tell the system the transforms on the object have been recycled.
    private bool positionsSet = false;

    // Hold the cells this building occupies
    [HideInInspector] public List<Vector2Int> cells;

    // Holds the rotation value for comparisons
    public enum RotationType
    {
        NORTH = 1,
        EAST = 2,
        SOUTH = 3,
        WEST = 4
    }
    [HideInInspector] public RotationType rotation;

    // Called by building handler if additional options should be applied
    public virtual void ApplyOptions(int option)
    {
        Debug.Log("This building does not have optional values!");
    }

    // Called by another building to input an entity
    public virtual bool InputEntity(Entity entity)
    {
        Debug.Log("This building cannot input entities!");
        return false;
    }

    // Called once an entity gets to the input position
    public virtual void ReceiveEntity(Entity entity)
    {
        Debug.Log("This building cannot receive entities!");
    }

    // Called once an entity is ready to be output
    public virtual void OutputEntity(Entity entity)
    {
        Debug.Log("This building cannot output entities!");
    }

    public void MoveInput(IOClass input, IOClass output)
    {
        acceptingEntities = true;
        input.reserved = false;
        input.bin = null;
        output.reserved = true;
        input.target.UpdateBins();
    }

    public void MoveOutput(IOClass output)
    {
        output.reserved = false;
        output.bin = null;
    }

    // Used to update the bins on a building
    //
    // Ex. A conveyor has a front bin which contains the entity sitting at the front side of
    // the conveyor. If tile the conveyor is on is updated as an input tile (meaning a building
    // nearby can take entities from that tile), this function will get called. It will continue
    // being called until all affected buildings are updated.
    public virtual void UpdateBins()
    {
        Debug.Log("This building does not contain bins to update");
    }

    // Used to pass a target to another building
    //
    // It is important to note that this can be an array of size 1. The next building may only
    // have one input and output. That is why this method exists, so that each building can
    // decide what to do with the targets it receives.
    public virtual bool SetInputTarget(Building target)
    {
        Debug.Log("This building cannot be passed input targets!");
        return false;
    }

    public virtual bool SetOutputTarget(Building target)
    {
        Debug.Log("This building cannot be passed output targets!");
        return false;
    }

    // Used to setup the rotation of a building
    //
    // Buildings will often need to make rotational checks in order to make sure that the
    // adjacent tile can pass items on to it. Conveyors have additional built in rotation
    // functionality that allows them to offset their rotation if the conveyor is on an
    // angle. This adjustment does not need to be accounted for with this system.
    //
    // Only call this if needed. Some buildings may not care which way they're oriented.
    public virtual void SetupRotation()
    {
        if (transform.rotation.eulerAngles.z == 0f) rotation = RotationType.EAST;
        else if (transform.rotation.eulerAngles.z == 90f) rotation = RotationType.NORTH;
        else if (transform.rotation.eulerAngles.z == 180f) rotation = RotationType.WEST;
        else if (transform.rotation.eulerAngles.z == 270f) rotation = RotationType.SOUTH;
    }

    // Checks for nearby buildings
    public void CheckNearbyBuildings()
    {
        // Loop through each input
        for (int i = 0; i < inputs.Length; i++)
        {
            Building building = BuildingHandler.active.TryGetBuilding(inputs[i].tilePosition);
            if (building != null)
            {
                if (skipInputCheck || building.skipOutputCheck || building.rotation == rotation)
                {
                    if (!building.SetOutputTarget(this)) break;
                    SetInputTarget(building);
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
            Building building = BuildingHandler.active.TryGetBuilding(outputs[i].tilePosition);

            if (building != null)
            {
                if (skipOutputCheck || building.skipInputCheck || building.rotation == rotation)
                {
                    if (!building.SetInputTarget(this)) break;
                    SetOutputTarget(building);
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
        {
            inputs[i].position = inputs[i].transform.position;
            inputs[i].tilePosition = inputs[i].tile.position;
            Recycler.AddRecyclable(inputs[i].transform);
            Recycler.AddRecyclable(inputs[i].tile);
        }

        // Setup output positions
        for (int i = 0; i < outputs.Length; i++)
        {
            outputs[i].position = outputs[i].transform.position;
            outputs[i].tilePosition = outputs[i].tile.position;
            Recycler.AddRecyclable(outputs[i].transform);
            Recycler.AddRecyclable(outputs[i].tile);
        }

        positionsSet = true;
    }

    // Checks if building hologram is in an output position
    public bool CheckOutputPosition(Transform obj)
    {
        foreach (IOClass output in outputs)
            if (output.tilePosition == obj.position)
                return true;
        return false;
    }

    // Destroys the entity
    public void Kill()
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
