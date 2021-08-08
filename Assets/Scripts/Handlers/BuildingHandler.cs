using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingHandler : MonoBehaviour
{
    public Tilemap grid;
    public Item test;
    public static BuildingHandler active;

    // Start method grabs tilemap
    private void Start()
    {
        if (grid == null)
            Debug.LogError("Scene needs a tilemap to support building!");
        active = this;
    }

    // Update is called once per frame
    private void Update()
    {
        // Round to grid
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
    }

    // Register a tile 
    public void GenerateTile()
    {
        Vector3Int cellIndex = grid.WorldToCell(transform.position);
        grid.SetTile(cellIndex, test);
        Debug.Log("Placed tile at " + transform.position);
    }
}
