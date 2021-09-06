using UnityEngine;
using Mirror;
using System;

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

    // Flag to tell the system the transforms on the object have been recycled.
    private bool positionsSet = false;

    // Holds the rotation value for comparisons
    public enum RotationType
    {
        NORTH = 1,
        EAST = 2,
        SOUTH = 3,
        WEST = 4
    }
    [HideInInspector] public RotationType rotation;

    // Used to pass another building an entity. 
    //
    // Ex. A conveyor has an entity in the front bin and wants to get rid of it. It will
    // look at the next building in front of it and call it's pass entity method. That
    // building can then choose what to do with it, but if it returns false then the entity
    // will stay put with the conveyor until it gets called again.
    public virtual bool PassEntity(Entity entity)
    {
        Debug.Log("This building cannot be passed entities!");
        return false;
    }

    // Used to tell a building what to do with an entity once it has it
    //
    // Ex. A conveyor has passed a building an entity. The entity has just arrived at the 
    // input position. This method will then determine what to do with the entity at that time.
    // Similar to PassEntity(), but it waits until the entity is at the input position first.
    // For conveyors, this is used to update the entity bins.
    public virtual void ReceiveEntity(Entity entity)
    {
        Debug.Log("This building cannot receive entities!");
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
    public virtual void SetInputTarget(Building target)
    {
        Debug.Log("This building cannot be passed input targets!");
    }

    public virtual void SetOutputTarget(Building target)
    {
        Debug.Log("This building cannot be passed output targets!");
    }

    // Used to setup the rotation of a building
    //
    // Buildings will often need to make rotational checks in order to make sure that the
    // adjacent tile can pass items on to it. Conveyors have additional built in rotation
    // functionality that allows them to offset their rotation if the conveyor is on an
    // angle. This adjustment does not need to be accounted for with this system.
    //
    // Only call this if needed. Some buildings may not care which way they're oriented.
    public void SetupRotation()
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
                Debug.Log(rotation + " = " + building.rotation);

                if (building.rotation == rotation)
                {
                    building.SetOutputTarget(this);
                    building.UpdateBins();
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
                Debug.Log(building.name);

                if (building.rotation == rotation)
                {
                    building.SetInputTarget(this);
                    SetOutputTarget(building);
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
}
