using Mirror;
using UnityEngine;

public class BuildingController : NetworkBehaviour
{
    // Selected tile
    public Transform hologram;
    public Transform hologramInput;
    public Transform hologramOutput;
    private Tile building;
    public int option = -1;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    public void Start()
    {
        // Confirm user has authority
        if (!hasAuthority) return;

        // Grab sprite renderer component
        spriteRenderer = hologram.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        // Confirm user has authority
        if (!hasAuthority) return;

        // Update position and sprite transparency
        UpdatePosition();
        AdjustTransparency();
        CheckInput();
    }

    [Client]
    public void CheckInput()
    {
        // Clicking input check
        if (Input.GetKey(Keybinds.shoot))
        {
            if (building != null) CmdCreateBuilding();
            else
            {
                Building holder = BuildingHandler.active.TryGetBuilding(hologram.position);
                if (holder != null) Events.active.BuildingClicked(holder);
            }
        }
        else if (Input.GetKey(Keybinds.deselect)) CmdDestroyBuilding();
        else if (Input.GetKeyDown(Keybinds.rotate)) RotatePosition();
        else if (Input.GetKeyDown(Keybinds.deselect)
            || Input.GetKeyDown(Keybinds.escape)) SetBuilding(null);
    }

    // Create building (command)
    [Command]
    public void CmdCreateBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.CreateBuilding(building, hologram.position, hologram.rotation, option);
        else Debug.LogError("Scene does not have active building handler!");

        // temp
        if (building.name == "Conveyor")
        {
            if (option == 1) hologram.Rotate(0, 0, 90);
            else if (option == 2) hologram.Rotate(0, 0, -90);
            SetBuilding(building);
        }
    }

    // Delete building (command)
    [Command]
    public void CmdDestroyBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.DestroyBuilding(hologram.position);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Sets the selected building (null to deselect)
    public void SetBuilding(Tile tile)
    {
        // Set tile 
        building = tile;
        option = -1;
        
        if (tile != null)
        {
            // Set option
            if (tile.hasOptions) option = 0;

            // Get the tile sprite and set offset
            spriteRenderer.sprite = SpritesManager.GetSprite(tile.name);

            // If tile isnt rotatable, reset rotation
            if (!tile.rotatable)
                hologram.rotation = Quaternion.identity;
        }
        else spriteRenderer.sprite = SpritesManager.GetSprite("Empty");
    }

    // Uses the offset value from the Tile SO to center the object
    private void UpdatePosition()
    {
        // Update position to mouse pointer
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (building != null) hologram.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + building.offset.x, 5 * Mathf.Round(mousePos.y / 5) + building.offset.y);
        else hologram.position = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
    }

    // Rotates an object
    private void RotatePosition()
    {
        if (building.obj.GetComponent<Conveyor>() != null)
        {
            Building inputBuilding = BuildingHandler.active.TryGetBuilding(hologramInput.position);
            if (inputBuilding != null)
            {
                if (option == 2) { spriteRenderer.sprite = SpritesManager.GetSprite("Conveyor"); option = 0; }
                else if (option == 1) { spriteRenderer.sprite = SpritesManager.GetSprite("Corner Down"); option = 2; }
                else { spriteRenderer.sprite = SpritesManager.GetSprite("Corner Up"); option = 1; }
            }
            else hologram.Rotate(0, 0, -90);
        }
        else if (building != null && building.rotatable)
        {
            hologram.Rotate(0, 0, -90);
        }
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
}
