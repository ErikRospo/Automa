using UnityEngine;
using Mirror;
using System.Collections.Generic;

// TODO: Fix scriptable object reference in commands

public class BuildingHandler : NetworkBehaviour
{
    // Grid variable
    [HideInInspector] public Grid tileGrid;

    // Building variables
    public static BuildingHandler active;
    public bool debugMode;
    public GameObject debugObj;

    public void Start() 
    {
        // Get reference to active instance
        active = this;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
    }

    // Creates a building
    public void CreateBuilding(BuildingData tile, Vector3 position, Quaternion rotation, int option)
    {
        // Untiy is so fucky it is now in a new dimension of bullshit
        if (tile == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles(tile, position, rotation)) return;

        // Instantiate the object like usual
        RpcInstantiateBuilding(tile, position, rotation, option);
    }

    //[ClientRpc]
    private void RpcInstantiateBuilding(BuildingData tile, Vector2 position, Quaternion rotation, int option)
    {
        // Create the tile
        Building lastBuilding = Instantiate(tile.obj, position, rotation).GetComponent<Building>();
        lastBuilding.transform.position = new Vector3(position.x, position.y, -1);
        lastBuilding.name = tile.name;
        lastBuilding.data = tile;

        // Apply options if required
        if (option != -1) lastBuilding.ApplyOptions(option);

        // Set the tiles on the grid class
        if (tile.cells.Length > 0)
        {
            foreach (BuildingData.Cell cell in tile.cells)
            {
                // Set cells based on rotation
                Vector2 cellPos = RotateVector(lastBuilding.transform.position, new Vector2(cell.x, cell.y), rotation);
                tileGrid.SetCell(Vector2Int.RoundToInt(cellPos), true, tile, lastBuilding);

                // If debug mode enabled, display coordinates
                if (debugMode)
                {
                    Transform holder = Instantiate(debugObj, cellPos, Quaternion.identity).GetComponent<Transform>();
                    holder.position = new Vector3(holder.position.x, holder.position.y, holder.position.z - 1.5f);
                }
            }
        }
        else
        {
            // Set cell based on transform position
            tileGrid.SetCell(Vector2Int.RoundToInt(lastBuilding.transform.position), true, tile, lastBuilding);

            if (debugMode)
            {
                Transform holder = Instantiate(debugObj, lastBuilding.transform.position, Quaternion.identity).GetComponent<Transform>();
                holder.position = new Vector3(holder.position.x, holder.position.y, holder.position.z - 1.5f);
            }
        }
    }

    public Vector2 RotateVector(Vector2 original, Vector2 cell, Quaternion rotation)
    {
        // Determine rotational changes
        Vector2 cellPos;
        switch (rotation.eulerAngles.z)
        {
            case 90f:
                cellPos = new Vector2(original.x - cell.y, original.y + cell.x);
                break;
            case 180f:
                cellPos = new Vector2(original.x - cell.x, original.y - cell.y);
                break;
            case 270f:
                cellPos = new Vector2(original.x + cell.y, original.y - cell.x);
                break;
            default:
                cellPos = new Vector2(original.x + cell.x, original.y + cell.y);
                break;
        }
        return cellPos;
    }

    // Destroys a building
    [ClientRpc]
    public void RpcDestroyBuilding(Vector3 position)
    {
        tileGrid.DestroyCell(Vector2Int.RoundToInt(position));
    }

    // Checks to make sure tile(s) isn't occupied
    [Server]
    public bool CheckTiles(BuildingData tile, Vector3 position, Quaternion rotation)
    {
        // Tells system to check tile placement
        bool checkTilePlacement = tile.spawnableOn.Count > 0;

        if (tile.cells.Length > 0)
        {
            foreach (BuildingData.Cell cell in tile.cells)
            {
                // Check to make sure nothing occupying tile
                Vector2Int coords = Vector2Int.RoundToInt(RotateVector(position, new Vector2(cell.x, cell.y), rotation));

                // Attempt to retrieve the cell
                if (tileGrid.RetrieveCell(coords) != null)
                    return false;

                // Check to make sure tile can be placed
                if (checkTilePlacement)
                {
                    if (WorldGen.active.spawnedResources.TryGetValue(coords, out MineralData value) &&
                        tile.spawnableOn.Contains(value)) checkTilePlacement = false;
                }
            }
        }
        else
        {
            // Check to make sure nothing occupying tile
            Vector2Int coords = Vector2Int.RoundToInt(new Vector2(position.x, position.y));
            if (tileGrid.RetrieveCell(coords) != null)
                return false;

            // Check to make sure tile can be placed
            if (checkTilePlacement)
            {
                if (WorldGen.active.spawnedResources.TryGetValue(coords, out MineralData value) &&
                    tile.spawnableOn.Contains(value)) checkTilePlacement = false;
            }
        }

        if (checkTilePlacement) return false;
        else return true;
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
