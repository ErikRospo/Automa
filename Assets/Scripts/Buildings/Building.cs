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
    public bool acceptingEntities = false;

    // Input / output positions
    //
    // These are used to define where an entity should travel to and from. For
    // example, the smelter has a conveyor hole on the top left of the sprite.
    // To actually tell the system to use that as an input, you'd create a new
    // transform and align it to how you want it. The system would then recycle
    // that transform at runtime and save it's position. Same deal for outputs.

    public Transform[] inputs;
    public Transform[] outputs;
    protected Vector3[] inputPositions;
    protected Vector3[] outputPositions;

    // Input / output tiles
    //
    // These are used to define where a building should check for other tiles.
    // Thanks to the grid system, this is as easy as dragging a transform over
    // to where you wanna check a grid point. The system will then recycle that
    // transform at runtime and save it's position. Then when a call is made to
    // the grid system, it will automatically round to the tile you chose. Easy!

    public Transform[] inputTiles;
    public Transform[] outputTiles;
    protected Vector3[] inputTilePositions;
    protected Vector3[] outputTilePositions;

    [HideInInspector] public Entity frontBin;
    [HideInInspector] public Entity rearBin;

    [HideInInspector] public Building nextTarget;
    [HideInInspector] public Building previousTarget;

    [HideInInspector] public bool inputReserved;
    [HideInInspector] public bool outputReserved;

    // Just a flag to tell the system the transforms on the object have been recycled.
    private bool positionsSet = false;

    // Holds the rotation value for comparisons
    public enum rotationType
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }
    public rotationType rotation;

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
        if (transform.rotation.eulerAngles.z == 0f) rotation = rotationType.EAST;
        else if (transform.rotation.eulerAngles.z == 90f) rotation = rotationType.NORTH;
        else if (transform.rotation.eulerAngles.z == 180f) rotation = rotationType.WEST;
        else if (transform.rotation.eulerAngles.z == 270f) rotation = rotationType.SOUTH;
    }

    // Checks for nearby buildings
    public void CheckNearbyBuildings()
    {
        // Check the front bin
        Conveyor conveyor = BuildingHandler.TryGetConveyor(outputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.rotation == rotation)
            {
                conveyor.previousTarget = this;
                nextTarget = conveyor;
            }
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
            if (building != null)
                nextTarget = building;
        }

        // Check the rear bin
        conveyor = BuildingHandler.TryGetConveyor(inputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.rotation == rotation)
            {
                conveyor.nextTarget = this;
                conveyor.UpdateBins();
                previousTarget = conveyor;
            }
            else if (conveyor.isCorner) conveyor.CornerCheck(this);
        }
        else
        {
            Building building = BuildingHandler.TryGetBuilding(inputTilePositions[0]);
            if (building != null)
            {
                building.nextTarget = this;
                building.UpdateBins();
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

        // Tell the system how many targets it will need to look for
        // inputTargets = new Building[inputTilePositions.Length];
        // outputTargets = new Building[outputTilePositions.Length];

        // Setup input positions
        inputPositions = new Vector3[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputPositions[i] = inputs[i].position;
            Recycler.AddRecyclable(inputs[i]);
        }
        inputs = Array.Empty<Transform>();

        // Setup output positions
        outputPositions = new Vector3[outputs.Length];
        for (int i = 0; i < outputs.Length; i++)
        {
            outputPositions[i] = outputs[i].position;
            Recycler.AddRecyclable(outputs[i]);
        }
        outputs = Array.Empty<Transform>();

        // Setup input positions
        inputTilePositions = new Vector3[inputTiles.Length];
        for (int i = 0; i < inputTiles.Length; i++)
        {
            inputTilePositions[i] = inputTiles[i].position;
            Recycler.AddRecyclable(inputTiles[i]);
        }
        inputTiles = Array.Empty<Transform>();

        // Setup input positions
        outputTilePositions = new Vector3[outputTiles.Length];
        for (int i = 0; i < outputTiles.Length; i++)
        {
            outputTilePositions[i] = outputTiles[i].position;
            Recycler.AddRecyclable(outputTiles[i]);
        }
        outputTiles = Array.Empty<Transform>();

        positionsSet = true;
    }
}
