using UnityEngine;

[System.Serializable]
public class IOClass
{ 
    public Vector2 binPosition; // Location of where entity will get moved to
    public Vector2 binConnector; // Location of where building should check for a connector
    public Vector2 binTarget; // Locaiton of where building should check for another building

    [HideInInspector] public Item bin; // Holds an entity
    [HideInInspector] public Building building; // Target building. 
    [HideInInspector] public IOClass target; // Target (either input or output)
    [HideInInspector] public bool reserved; // Flag for other entities to determine if this intput / output is reserved

    // Setup the positions and recycle transforms
    public void SetupPosition(Vector2 position, float rotation)
    {
        switch(rotation)
        {
            case 90f:
                binPosition = new Vector2(-binPosition.y, binPosition.x);
                binConnector = new Vector2(-binConnector.y, binConnector.x);
                binTarget = new Vector2(-binTarget.y, binTarget.x);
                break;
            case 180f:
                binPosition = new Vector2(-binPosition.x, -binPosition.y);
                binConnector = new Vector2(-binConnector.x, -binConnector.y);
                binTarget = new Vector2(-binTarget.x, -binTarget.y);
                break;
            case 270f:
                binPosition = new Vector2(binPosition.y, -binPosition.x);
                binConnector = new Vector2(binConnector.y, -binConnector.x);
                binTarget = new Vector2(binTarget.y, -binTarget.x);
                break;
        }

        binPosition = new Vector2(position.x + binPosition.x, position.y + binPosition.y);
        binConnector = new Vector2(position.x + binConnector.x, position.y + binConnector.y);
        binTarget = new Vector2(position.x + binTarget.x, position.y + binTarget.y);
    }

    // Rotate positions (mainly for conveyors)
    public void RotatePosition(bool rotateUp)
    {
        if (rotateUp)
        {
            binPosition = new Vector2(0, binPosition.x);
            binConnector = new Vector2(0, binConnector.x);
            binTarget = new Vector2(0, binTarget.x);
        }
        else
        {
            binPosition = new Vector2(0, -binPosition.x);
            binConnector = new Vector2(0, -binConnector.x);
            binTarget = new Vector2(0, -binTarget.x);
        }
    }
}
