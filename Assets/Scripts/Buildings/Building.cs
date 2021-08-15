using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Building : NetworkBehaviour, IDamageable
{
    public bool acceptingEntities = false;
    public Transform[] inputPositions;
    public Transform[] outputPositions;
    public bool inputReserved;
    public bool outputReserved;

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
}
