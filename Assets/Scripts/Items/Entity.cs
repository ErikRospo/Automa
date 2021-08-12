using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Item item;
    private ConveyorHandler.ConveyorEntity holder;

    public void SetConveyorTarget(float speed, Vector3 target, Conveyor conveyor)
    {
        holder = ConveyorHandler.RegisterConveyorEntity(speed, target, this, conveyor);
    }

    public void RemoveConveyorTarget(bool removeEntity)
    {
        ConveyorHandler.RemoveConveyorEntity(holder);

        if (removeEntity) Destroy(gameObject);
        else holder = null;
    }
}
