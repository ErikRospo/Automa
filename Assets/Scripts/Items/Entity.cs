using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Item item;
    private ConveyorHandler.ConveyorEntity holder;

    // Parameterized constructor
    public Entity(Item item)
    {
        this.item = item;
        holder = null;
    }

    public void SetTarget(float speed, Vector3 target)
    {
        holder = ConveyorHandler.RegisterConveyorEntity(speed, target, transform);
    }

    public void RemoveTarget(bool removeEntity)
    {
        ConveyorHandler.RemoveConveyorEntity(holder);

        if (removeEntity) Destroy(gameObject);
        else holder = null;
    }
}
