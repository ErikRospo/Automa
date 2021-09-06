using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingHandler : NetworkBehaviour
{
    // Grid variable
    [HideInInspector] public Grid tileGrid;

    // Building variables
    public static BuildingHandler active;
    private Tile selectedTile;
    private Vector2 position;
    private Vector2 offset;
    private Quaternion rotation;
    private GameObject lastObj;
    public bool holdingMouse;

    // Axis Lock variables
    private SnapAxis snapAxis;
    private Vector3 snapPos;

    // Conveyor variables
    private Conveyor lastConveyor;
    private Vector2 lastConveyorPosition;
    private bool conveyorCorner;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;
    private bool conveyorRotateSwitch;

    // Start method grabs tilemap
    private void Start()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        if (active) spriteRenderer = GetComponent<SpriteRenderer>();

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if active is null
        if (active == null) return;

        // Round to grid
        OffsetBuilding();
        rotation = active.transform.rotation;

        AdjustTransparency();
    }

    // Uses the offset value from the Tile SO to center the object
    private void OffsetBuilding()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        active.transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + offset.x, 5 * Mathf.Round(mousePos.y / 5) + offset.y);
        
        // If active is a axisSnappable and mouse is held, snap axis
        if (selectedTile.axisSnappable && holdingMouse)
        {
            Snapping.Snap(active.transform.position, snapPos, snapAxis);
        }
        position = active.transform.position;

        // Update last conveyor position (for rotating)
        if (conveyorCorner == true && lastConveyorPosition != position)
        {
            spriteRenderer.sprite = Sprites.GetSprite(selectedTile.name);
            conveyorCorner = false;
        }
    }

    // Rotates an object
    public void Rotate()
    {
        if (selectedTile != null && (selectedTile.obj.GetComponent<Conveyor>() == null || !ConveyorRotationCheck()))
            active.transform.Rotate(0, 0, -90);
    }

    // Automatically applies corner rotation to conveyors
    private bool ConveyorRotationCheck()
    {
        // If position has not moved since last check, don't reset target tile
        if (lastConveyorPosition != position)
        {
            Vector2 targetTile;
            switch (rotation.eulerAngles.z)
            {
                case 90f:
                    targetTile = new Vector2(position.x, position.y - 5f);
                    break;
                case 180f:
                    targetTile = new Vector2(position.x + 5f, position.y);
                    break;
                case 270f:
                    targetTile = new Vector2(position.x, position.y + 5f);
                    break;
                default:
                    targetTile = new Vector2(position.x - 5f, position.y);
                    break;
            }

            // If conveyor found, save it and set corner sprite
            lastConveyor = TryGetConveyor(targetTile);
            if (lastConveyor != null)
            {
                spriteRenderer.sprite = Sprites.GetSprite("ConveyorTurnRight");
                conveyorCorner = true;
            }
            lastConveyorPosition = position;
            conveyorRotateSwitch = false;
        }

        // If the previous conveyor still exists, rotate based off it's orientation
        if (lastConveyor != null)
        {
            if (conveyorRotateSwitch)
            {
                spriteRenderer.sprite = Sprites.GetSprite("ConveyorTurnRight");
                conveyorRotateSwitch = false;
            }
            else
            {
                spriteRenderer.sprite = Sprites.GetSprite("ConveyorTurnLeft");
                conveyorRotateSwitch = true;
            }
            return true;
        }
        return false;
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaHolder = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaHolder = alphaAdjust;

        // Set alpha
        spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + alphaHolder);
    }

    // Sets the selected building
    public void SetBuilding(Tile tile)
    {
        // Check if active is null
        if (active == null) return;
        selectedTile = tile;

        if (tile != null)
        {
            spriteRenderer.sprite = Sprites.GetSprite(tile.name);
            offset = tile.offset;
        }
        else spriteRenderer.sprite = Sprites.GetSprite("Empty");
    }

    // Creates a building
    public void CmdCreateBuilding()
    {
        // Check if active is null
        if (active == null || selectedTile == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Check that conveyors are being placed in axis if mouse is being held down
        if (lastObj != null)
        {
            Conveyor lastConveyor = lastObj.GetComponent<Conveyor>(); ;
            if (holdingMouse && lastConveyor != null)
            {
                Vector2 nextPositon = new Vector2(lastConveyor.outputs[0].tilePosition.x, lastConveyor.outputs[0].tilePosition.y);
                Vector2 previousPostion = new Vector2(lastConveyor.inputs[0].tilePosition.x, lastConveyor.inputs[0].tilePosition.y);
                Debug.Log(previousPostion.ToString() + " | " + position.ToString());
                if (nextPositon != position && previousPostion != position) return;
            }
        }

        // Instantiate the object like usual
        InstantiateObj(selectedTile.obj, position, active.transform.rotation);

        // Set the tiles on the grid class
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(lastObj.transform.position.x + cell.x, lastObj.transform.position.y + cell.y)), true, selectedTile, lastObj);
        }
        else tileGrid.SetCell(Vector2Int.RoundToInt(lastObj.transform.position), true, selectedTile, lastObj);
    }

    private void InstantiateObj(GameObject obj, Vector2 position, Quaternion rotation, int axisLock = -1)
    {
        // Create the tile
        lastObj = Instantiate(obj, position, rotation);
        lastObj.name = obj.name;
        SpriteRenderer spriteRenderer = lastObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.sprite = Sprites.GetSprite(obj.name);

        // Conveyor override creation
        Conveyor conveyor = lastObj.GetComponent<Conveyor>();
        if (conveyor != null)
        {
            if (conveyorCorner)
            {
                conveyor.ToggleCorner(conveyorRotateSwitch);

                if (conveyorRotateSwitch) active.transform.Rotate(new Vector3(0, 0, 90));
                else active.transform.Rotate(new Vector3(0, 0, -90));
            }
            conveyor.Setup();
        }
    }

    // Checks to make sure tile(s) isn't occupied
    public bool CheckTiles()
    {
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                if (tileGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y))) != null)
                    return false;
        }
        else return tileGrid.RetrieveCell(Vector2Int.RoundToInt(position)) == null;
        return true;
    }

    // Attempts to return a building
    public Building TryGetBuilding(Vector2 position)
    {
        Grid.Cell cell = tileGrid.RetrieveCell(Vector2Int.RoundToInt(position));
        if (cell != null)
        {
            Building building = cell.obj.GetComponent<Building>();
            return building;
        }
        return null;
    }

    // Attempts to return a conveyor
    public Conveyor TryGetConveyor(Vector2 position)
    {
        Grid.Cell cell = tileGrid.RetrieveCell(Vector2Int.RoundToInt(position));
        if (cell != null)
        {
            Conveyor conveyor = cell.obj.GetComponent<Conveyor>();
            return conveyor;
        }
        return null;
    }
}
