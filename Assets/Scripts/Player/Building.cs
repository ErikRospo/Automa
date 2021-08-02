using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Vector2 MousePos;

    // Update is called once per frame
    void Update()
    {
        // Round position to grid
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5 * Mathf.Round(MousePos.x / 5), 5 * Mathf.Round(MousePos.y / 5));

        // Attempts to place a building
        if (Input.GetButton("Fire1")) PlaceBuilding();
    }

    private void PlaceBuilding()
    {
        
    }
}
