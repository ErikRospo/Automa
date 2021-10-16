using Mirror;
using UnityEngine;

public class BuildingController : NetworkBehaviour
{
    // Selected tile
    public Transform hologram;
    private Tile building;

    // Conveyor variables
    private Conveyor lastConveyor;
    private Vector3 lastConveyorPosition;
    private bool conveyorCorner;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;
    private bool conveyorRotateSwitch;

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
            else Debug.Log("Get tile info");
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
            BuildingHandler.active.CreateBuilding(building, hologram.position, hologram.rotation);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Delete building (command)
    [Command]
    public void CmdDestroyBuilding()
    {
        if (BuildingHandler.active != null)
            BuildingHandler.active.DestroyBuilding(hologram.position);
        else Debug.LogError("Scene does not have active building handler!");
    }

    // Sets the selected building
    public void SetBuilding(Tile tile)
    {
        // Set tile (pass null to deselect)
        building = tile;
        if (tile != null)
        {
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

        /* Update last conveyor position (for rotating)
        if (conveyorCorner == true && lastConveyorPosition != building.position)
        {
            spriteRenderer.sprite = Sprites.GetSprite(selectedTile.name);
            conveyorCorner = false;
        }
        */
    }

    // Rotates an object
    private void RotatePosition()
    {
        if (building != null && building.rotatable &&
            (building.obj.GetComponent<Conveyor>() == null || !ConveyorRotationCheck()))
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




























    // CODE HELL. CODE COMES HERE TO DIE. SMILE.

    // Automatically applies corner rotation to conveyors
    private bool ConveyorRotationCheck()
    {
        // If position has not moved since last check, don't reset target tile
        if (lastConveyorPosition != hologram.position)
        {
            Vector2 targetTile;
            switch (hologram.rotation.eulerAngles.z)
            {
                case 90f:
                    targetTile = new Vector2(hologram.position.x, hologram.position.y - 5f);
                    break;
                case 180f:
                    targetTile = new Vector2(hologram.position.x + 5f, hologram.position.y);
                    break;
                case 270f:
                    targetTile = new Vector2(hologram.position.x, hologram.position.y + 5f);
                    break;
                default:
                    targetTile = new Vector2(hologram.position.x - 5f, hologram.position.y);
                    break;
            }

            // If conveyor found, save it and set corner sprite
            lastConveyor = BuildingHandler.active.TryGetConveyor(targetTile);
            if (lastConveyor != null)
            {
                spriteRenderer.sprite = SpritesManager.GetSprite("ConveyorTurnRight");
                conveyorCorner = true;
            }
            lastConveyorPosition = hologram.position;
            conveyorRotateSwitch = false;
        }

        // If the previous conveyor still exists, rotate based off it's orientation
        if (lastConveyor != null)
        {
            if (conveyorRotateSwitch)
            {
                spriteRenderer.sprite = SpritesManager.GetSprite("ConveyorTurnRight");
                conveyorRotateSwitch = false;
            }
            else
            {
                spriteRenderer.sprite = SpritesManager.GetSprite("ConveyorTurnLeft");
                conveyorRotateSwitch = true;
            }
            return true;
        }
        return false;
    }

    /* Conveyor override creation
Conveyor conveyor = lastBuilding.GetComponent<Conveyor>();
if (conveyor != null)
{
    if (conveyorCorner)
    {
        conveyor.ToggleCorner(conveyorRotateSwitch);

        if (conveyorRotateSwitch) active.building.Rotate(new Vector3(0, 0, 90));
        else active.building.Rotate(new Vector3(0, 0, -90));
    }
    conveyor.Setup();
}
*/
}
