using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Item item;
    public IOClass holder;

    public void MoveTo(float speed, IOClass target, Building building, bool output = false)
    {
        holder = target;
        EntityHandler.active.RegisterMovingEntity(speed, target.position, this, building, output);
    }

    public void MoveTo(float speed, Vector3 target, Building building, bool output = false)
    {
        EntityHandler.active.RegisterMovingEntity(speed, target, this, building, output);
    }
}
