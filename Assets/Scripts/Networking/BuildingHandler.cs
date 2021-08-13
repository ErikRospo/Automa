using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingHandler : NetworkBehaviour
{
    // Grid variable
    public static Grid tileGrid;

    // Building variables
    public static BuildingHandler active;
    private static Tile selectedTile;
    private static Vector2 position;
    private static Quaternion rotation;
    private static GameObject lastObj;
    private static bool changeSprite;

    // Axis variables
    public static bool buildPressed;
    public static float lockAxisX, lockAxisY;

    // Sprite values
    private static SpriteRenderer spriteRenderer;
    private static float alphaAdjust = 0.005f;
    private static float alphaHolder;

    // Start method grabs tilemap
    private void Start()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2, Grid.Cell>();
        selectedTile = null;
        position = new Vector2(0, 0);
        rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        changeSprite = false;
        lastObj = null;

        // Sets static anim variables
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaHolder = alphaAdjust;
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if active is null
        if (active == null) return;

        // Round to grid
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
        position = active.transform.position;
        rotation = active.transform.rotation;

        AdjustTransparency();
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Check if building changed
        if (changeSprite)
        {
            try
            {
                if (selectedTile != null) spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Buildings/" + selectedTile.name);
                else spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Interface/Empty");
            }
            catch
            {
                Debug.LogError("Sprite could not be retrieved. Please check you have placed the sprite in resources with the correct name!");
            }
            changeSprite = false;
        }

        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaHolder = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaHolder = alphaAdjust;

        // Set alpha
        spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + alphaHolder);
    }

    // Sets the selected building
    public static void SetBuilding(Tile tile)
    {
        // Check if active is null
        if (active == null) return;

        changeSprite = true;
        selectedTile = tile;
    }

    // Creates a building
    public static void CmdCreateBuilding()
    {
        // Check if active is null
        if (active == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Set build pressed
        if (!buildPressed)
        {
            buildPressed = true;
            InstantiateObj(selectedTile.obj, position, active.transform.rotation);
        }
        else if (selectedTile.name == "Conveyor")
        {
            if (lockAxisX == -1 && lockAxisY == -1 && lastObj.transform.position.x == position.x)
            {
                lockAxisX = position.x;
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Interface/Empty");
            }
            else if (lockAxisX == -1 && lockAxisY == -1 && lastObj.transform.position.y == position.y)
            {
                lockAxisY = position.y;
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Interface/Empty");
            }

            // Create the tile
            if (lockAxisX != -1) InstantiateObj(selectedTile.obj, new Vector2(lockAxisX, position.y), active.transform.rotation);
            else if (lockAxisY != -1) InstantiateObj(selectedTile.obj, new Vector2(position.x, lockAxisY), active.transform.rotation);
            else InstantiateObj(selectedTile.obj, position, active.transform.rotation);
        }
        else InstantiateObj(selectedTile.obj, position, active.transform.rotation);

        // Set the tiles on the grid class
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                tileGrid.SetCell(new Vector2(lastObj.transform.position.x + cell.x, lastObj.transform.position.y + cell.y), true, selectedTile, lastObj);
        }
        else tileGrid.SetCell(lastObj.transform.position, true, selectedTile, lastObj);
    }

    private static void InstantiateObj(GameObject obj, Vector2 position, Quaternion rotation)
    {
        // Create the tile
        lastObj = Instantiate(obj, position, rotation);
        lastObj.name = obj.name;
    }

    // Keybind for building released
    public static void BuildReleased()
    {
        buildPressed = false;
        lockAxisX = -1;
        lockAxisY = -1;
    }

    // Checks to make sure tile(s) isn't occupied
    public static bool CheckTiles()
    {
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                if (tileGrid.RetrieveCell(new Vector2(position.x + cell.x, position.y + cell.y)) != null)
                    return false;
        }
        else return tileGrid.RetrieveCell(position) == null;
        return true;
    }

    // Attempts to return a building
    public static Building TryGetBuilding(Vector2 position)
    {
        Grid.Cell cell = tileGrid.RetrieveCell(position);
        if (cell != null)
        {
            Building building = cell.obj.GetComponent<Building>();
            return building;
        }
        return null;
    }

    // Attempts to return a conveyor
    public static Conveyor TryGetConveyor(Vector2 position)
    {
        Grid.Cell cell = tileGrid.RetrieveCell(position);
        if (cell != null)
        {
            Debug.Log("Found cell at " + position);
            Conveyor conveyor = cell.obj.GetComponent<Conveyor>();
            return conveyor;
        }
        return null;
    }
}
