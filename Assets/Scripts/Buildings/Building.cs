using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public bool isConveyor;

    public virtual bool PassEntity(Entity entity)
    {
        Debug.Log("This building can not receive items");
        return false;
    }
}
