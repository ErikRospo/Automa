using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Item item;

    public void MoveTo(float speed, IOClass target, Building building, bool output = false)
    {
        EntityHandler.active.RegisterMovingEntity(speed, target, this, building, output);
    }
}
