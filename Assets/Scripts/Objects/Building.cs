using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Building : NetworkBehaviour, IDamageable
{
    public bool acceptingEntities = true;
    public virtual void PassEntity(Entity entity)
    {
        Debug.Log("This building can not receive items");
    }
}
