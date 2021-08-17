using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Item item;

    public void MoveTo(float speed, Vector3 target, Building building)
    {
        EntityHandler.RegisterMovingEntity(speed, target, this, building);
    }
}
