using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/Building")]
public abstract class Building : Tile
{
    public virtual bool PassEntity(Entity entity)
    {
        Debug.Log("This building can not receive items");
        return false;
    }
}
