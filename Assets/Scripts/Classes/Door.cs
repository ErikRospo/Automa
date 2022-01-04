using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BaseObject
{
    // Door variables
    public Door sibling;
    public bool isAutomatic;
    public bool canInteract = false;
    public Vector2 openPosition;
    public Vector2 closedPosition;
    public float doorSpeed;

    // Internal flags
    private bool isSubscribed = false;
    private bool isOpen = false;

    // Manually toggle the door
    public void ToggleDoor()
    {
        if (isOpen) CloseDoor();
        else OpenDoor();
    }

    // Open the door
    public void OpenDoor()
    {
        if (movingObject != null) EntityHandler.active.RemoveMovingObject(movingObject);
        movingObject = EntityHandler.active.RegisterMovingObject(this, doorSpeed, openPosition);
        if (sibling != null) sibling.OpenDoor();

        isOpen = !isOpen;
    }

    // Close the door
    public void CloseDoor()
    {
        if (movingObject != null) EntityHandler.active.RemoveMovingObject(movingObject);
        movingObject = EntityHandler.active.RegisterMovingObject(this, doorSpeed, closedPosition);
        if (sibling != null) sibling.CloseDoor();

        isOpen = !isOpen;
    }

    // Trigger area (can work without a trigger)
    public void OnTriggerEnter2D(Collider2D collision) 
    {
        if (isAutomatic) OpenDoor();
        else if (canInteract && !isSubscribed)
        {
            isSubscribed = true;
            InputEvents.active.onInteractPressed += ToggleDoor;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) 
    {
        if (isAutomatic) CloseDoor();
        else if (canInteract && isSubscribed)
        {
            isSubscribed = false;
            InputEvents.active.onInteractPressed -= ToggleDoor;
        }
    }

    // Override position arrive
    public override void FinishMoving()
    {
        
    }
}
