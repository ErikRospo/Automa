using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseObject
{
    // Door variables
    public Door sibling;
    public bool isOpen = false;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float doorSpeed;

    // Open the door
    public void OpenDoor()
    {
        if (movingObject != null) EntityHandler.active.RemoveMovingObject(movingObject);
        movingObject = EntityHandler.active.RegisterMovingObject(this, doorSpeed, openPosition);
        if (sibling != null) sibling.OpenDoor();
    }

    // Close the door
    public void CloseDoor()
    {
        if (movingObject != null) EntityHandler.active.RemoveMovingObject(movingObject);
        movingObject = EntityHandler.active.RegisterMovingObject(this, doorSpeed, closedPosition);
        if (sibling != null) sibling.CloseDoor();
    }

    // Trigger area (can work without a trigger)
    public void OnTriggerEnter2D(Collider2D collision) { OpenDoor(); }
    public void OnTriggerExit2D(Collider2D collision) { CloseDoor(); }

    // Override position arrive
    public override void FinishMoving()
    {
        isOpen = !isOpen;
    }
}
