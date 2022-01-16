using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the base class for all objects. Contains basic definitions,
// such as from the IDamageable interface, and for moving the object.

public class BaseObject : MonoBehaviour
{
    // Moving object instance
    public EntityHandler.MovingObject movingObject = null;

    // Uses the item
    public virtual void Use() { }

    // Called by the EntityHandler when an object arrives at its destination
    public virtual void FinishMoving() { }

    // Destroys the entity
    public virtual void Destroy()
    {
        // Destroy gameObject
        Destroy(gameObject);
    }
}
