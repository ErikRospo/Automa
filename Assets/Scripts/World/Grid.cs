using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    // Cell class. Holds info about each cell
    public class Cell
    {
        public Cell(bool occupied, Tile tile, Building obj)
        {
            this.occupied = occupied;
            this.tile = tile;
            this.obj = obj;
        }

        public bool occupied;
        public Tile tile;
        public Building obj;
    }

    // Holds a dictionary of all cells
    // Int represents coords of the tile 
    public Dictionary<Vector2Int, Cell> cells;

    // Grid values
    public int gridSize;
    public int cellSize;

    public Building RetrieveObject(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            return cell.obj;
        }
        else return null;
    }

    public Cell RetrieveCell(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            return cell;
        }
        else return null;
    }

    public void SetCell(Vector2Int coords, bool occupy, Tile tile = null, Building obj = null)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            cell.tile = tile;
            cell.occupied = occupy;
            cell.obj = obj;
        }
        else cells.Add(coords, new Cell(occupy, tile, obj));
        if (obj != null) obj.cells.Add(coords);
    }

    public void DestroyCell(Vector2Int coords) 
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            Building building = cell.obj.GetComponent<Building>();

            if (building != null)
            {
                for (int i = 0; i < building.cells.Count; i++)
                    cells.Remove(building.cells[i]);
                building.Kill();
            }
        }
    }
}
