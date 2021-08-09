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
    private static Dictionary<Vector2, Building> buildings;
    private static GameObject lastObj;
    private static bool changeSprite;

    public Building test;

    // Sprite values
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float alphaAdjust;
    private float alphaHolder;

    // Start method grabs tilemap
    private void Start()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        selectedBuilding = null;
        position = new Vector2(0, 0);
        rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        buildings = new Dictionary<Vector2, Building>();
        changeSprite = false;
        lastObj = null;
        alphaHolder = alphaAdjust;

        SetBuilding(test);
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if active is null
        if (active == null) return;

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
        // Check if building changed
        if (changeSprite)
        {
            try
            {
                if (selectedBuilding != null) spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Buildings/" + selectedBuilding.name);
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
        Debug.Log(spriteRenderer.color.a);
    }

    // Sets the selected building
    public static void SetBuilding(Building building)
    {
        // Check if active is null
        if (active == null) return;

        changeSprite = true;
        selectedBuilding = building;
    }

    // Creates a building
    public static void CmdCreateBuilding()
    {
        // Check if active is null
        if (active == null) return;

        if (buildings.ContainsKey(position)) return;
        lastObj = Instantiate(selectedBuilding.obj, position, rotation);
        lastObj.name = selectedBuilding.obj.name;
        buildings.Add(lastObj.transform.position, selectedBuilding);

        Debug.Log("Created building " + lastObj.name + " with key " + lastObj.transform.position);
    }
}
