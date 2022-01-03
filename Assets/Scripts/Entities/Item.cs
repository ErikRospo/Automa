using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData item;
    public Building lastBuilding;
    public int outputIndex;

    public void MoveTo(float speed, Vector3 target, Building building, bool output = false)
    {
        lastBuilding = building;
        EntityHandler.active.RegisterMovingEntity(speed, target, this, building, output);
    }
}
