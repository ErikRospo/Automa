using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Buildings script
//
// This is the parent script of all buildings. A lot of this functionality
// has to do with conveyors. You can define buildings to have multiple inputs
// and outputs, as well as which tiles they should check for other adjacent
// buildings. This is a very powerful system that allows for creation to be 
// handled easily through the editor, though additional functionality will
// need to be handled in seperate scripts.

public abstract class Building : NetworkBehaviour, IDamageable
{
    public bool acceptingEntities = false;

    // Inputs / outputs get destroyed
    public Transform[] inputs;
    public Transform[] outputs;
    protected Vector3[] inputPositions;
    protected Vector3[] outputPositions;

    // Input / output tiles get destroyed
    public Transform[] inputTiles;
    public Transform[] outputTiles;
    protected Vector3[] inputTilePositions;
    protected Vector3[] outputTilePositions;

    // Containers
    [HideInInspector] public Entity frontBin;
    [HideInInspector] public Entity rearBin;

    // Hold a reference to the previous belt
    public Building nextTarget;
    public Building previousTarget;

    public bool inputReserved;
    public bool outputReserved;
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


    // Must be called by each building
    // 
    // This method recycles all transforms on an object to alleviate memory / transform updates
    // by the engine. The object then holds a reference to those tiles via a Vector3. This saves
    // on the fly calculations in the future, and is a lot easier to setup and manage.
    public void SetupPositions()
    {
        if (positionsSet) return;

        // Setup input positions
        inputPositions = new Vector3[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputPositions[i] = inputs[i].position;
            Recycler.AddRecyclable(inputs[i]);
        }
        inputs = new Transform[0];

        // Setup output positions
        outputPositions = new Vector3[outputs.Length];
        for (int i = 0; i < outputs.Length; i++)
        {
            outputPositions[i] = outputs[i].position;
            Recycler.AddRecyclable(outputs[i]);
        }
        outputs = new Transform[0];

        // Setup input positions
        inputTilePositions = new Vector3[inputTiles.Length];
        for (int i = 0; i < inputTiles.Length; i++)
        {
            inputTilePositions[i] = inputTiles[i].position;
            Recycler.AddRecyclable(inputTiles[i]);
        }
        inputTiles = new Transform[0];

        // Setup input positions
        outputTilePositions = new Vector3[outputTiles.Length];
        for (int i = 0; i < outputTiles.Length; i++)
        {
            outputTilePositions[i] = outputTiles[i].position;
            Recycler.AddRecyclable(outputTiles[i]);
        }
        outputTiles = new Transform[0];

        positionsSet = true;
    }
}
