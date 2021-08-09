using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingHandler : NetworkBehaviour
{
    // Building variables
    public static BuildingHandler active;
    private static Building selectedBuilding;
    private static Vector2 position;
    private static Quaternion rotation;
    private static List<Transform> buildings;

    // Sprite values
    private static SpriteRenderer spriteRenderer;
    private float alphaAdjust;

    // Start method grabs tilemap
    private void Start()
    {
        active = this;
        position = new Vector2(0, 0);
        rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            alphaAdjust = 0.01f;
        }
        catch
        {
            Debug.Log("No building handler active in scene.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Round to grid
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
        position = transform.position;
        rotation = Quaternion.identity;

        AdjustTransparency();
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaAdjust = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaAdjust = -alphaAdjust;

        spriteRenderer.color = new Color(1f, 1f, 1f, alphaAdjust);
    }

    // Sets the selected building
    public static void SetBuilding(Building building)
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Buildings/" + building.name);
        selectedBuilding = building;
    }

    // Creates a building
    public static void CmdCreateBuilding()
    {
        GameObject holder = Instantiate(selectedBuilding.obj, position, rotation);
        holder.name = selectedBuilding.obj.name;
        buildings.Add(holder.transform);
    }
}
