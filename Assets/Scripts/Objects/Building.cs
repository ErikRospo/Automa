using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Building : NetworkBehaviour, IDamageable
{
    public bool acceptingEntities = false;
    public virtual bool PassEntity(Entity entity)
    {
        return acceptingEntities;
    }
}
