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

    public void Start() 
    {
        // Get reference to active instance
        active = this;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
    }

    // Creates a building
    public void CreateBuilding(Tile tile, Vector3 position, Quaternion rotation)
    {
        // Check if anything selected
        if (tile == null)
        {
            // Check for object
            Building obj = tileGrid.RetrieveObject(Vector2Int.RoundToInt(position));
            if (obj != null)
            {
                Constructor constructor = obj.GetComponent<Constructor>();
                if (constructor != null)
                    UIEvents.active.ConstructorClicked(constructor);
            }
            return;
        }

        // Check to make sure the tiles are not being used
        if (!CheckTiles(tile, position)) return;

        // Instantiate the object like usual
        RpcInstantiateBuilding(tile, position, rotation);
    }

    [ClientRpc]
    private void RpcInstantiateBuilding(Tile tile, Vector2 position, Quaternion rotation)
    {
        // Get game objected from scriptable manager
        GameObject obj = ScriptableManager.active.RequestBuildingByName(tile.name);
        if (obj == null) return;

        // Create the tile
        Building lastBuilding = Instantiate(obj, position, rotation).GetComponent<Building>();
        lastBuilding.name = tile.name;

        // Constructor override creation
        Constructor constructor = lastBuilding.GetComponent<Constructor>();
        if (constructor != null)
        {
            //Recipe temp = BuildingUI.active.recipes[BuildingUI.active.recipeSelector.value];
            //if (constructor.machine.recipes.Contains(temp)) constructor.recipe = temp;

            Debug.Log("Recipe setting not added");
        }

        // Set the tiles on the grid class
        if (tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in tile.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(lastBuilding.transform.position.x + cell.x, lastBuilding.transform.position.y + cell.y)), true, tile, lastBuilding);
        }
        else tileGrid.SetCell(Vector2Int.RoundToInt(lastBuilding.transform.position), true, tile, lastBuilding);
    }

    // Destroys a building
    public void DestroyBuilding(Vector3 position)
    {
        tileGrid.DestroyCell(Vector2Int.RoundToInt(position));
    }

    // Checks to make sure tile(s) isn't occupied
    public bool CheckTiles(Tile tile, Vector3 position)
    {
        if (tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in tile.cells)
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
