using HeathenEngineering.SteamAPI;
using Mirror;
using TMPro;
using UnityEngine;

// Handles anything player-input related

public class MovementController : NetworkBehaviour
{
    // Object animator 
    public Animator animator;

    // Building controller (attached to child object)
    private BuildingController buildingController;

    // Inventory script (attached to object)
    private Inventory inventory;

    // Player script (attached to object)
    private Player player;

    // Holds the equipped item
    public EntityData equippedItem;

    // GameObject child transforms
    private Rigidbody2D body;
    public Transform head;
    public Transform itemContainer;
    public Transform model;

    // Nametag variables
    [SyncVar(hook = nameof(OnNameChanged)), HideInInspector]
    public string playerName;
    public TextMeshProUGUI nameTag;

    // Movement variables
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 5f;
    private float horizontal;
    private float vertical;
    private float speed;

    // Chunking value
    private Vector2Int chunkCoords = new Vector2Int(0, 0);

    // Called on start
    [Client]
    void Start()
    {
        // Grab building script
        buildingController = GetComponent<BuildingController>();

        // Grab inventory script
        player = GetComponent<Player>();
        inventory = GetComponent<Inventory>();

        // Check to make sure scripts are found
        if (player == null) Debug.Log("[WARNING] No player script attached to Movement Controller!");
        if (inventory == null) Debug.Log("[WARNING] No inventory script attached to Movement Controller!");

        // Start for everyone
        body = GetComponent<Rigidbody2D>();
        inventory = GetComponent<Inventory>();
        
        // Initialize walk speed
        speed = walkSpeed;

        // Start for owner only
        if (hasAuthority)
        {
            Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
            CmdUpdateName(SteamSettings.Client.user.DisplayName);

            // Generate chunks
            if (WorldGen.active != null)
                WorldGen.active.UpdateChunks(chunkCoords);
        }
    }

    // Normal frame update
    [Client]
    void Update()
    {
        // Update for Everyone
        animator.SetFloat("Speed", speed);

        // Update for owner
        if (!hasAuthority || CameraFollow.freecam) return;

        // Rotates player body to mouse
        RotateToMouse();

        // Checks for keyboard input
        CheckInput();

        // Update the player for all other players
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        CmdUpdatePlayer(transform.position, model.localRotation.eulerAngles.z, head.rotation.eulerAngles.z, speed, GetComponent<Rigidbody2D>().velocity);

        // Neighbour chunk check
        UpdateChunks();
    }

    // Physics update for handling movement calculations 
    private void FixedUpdate()
    {
        if (!hasAuthority) return;

        var step = rotationSpeed * Time.deltaTime;
        if (speed > 0) model.localRotation = Quaternion.RotateTowards(model.localRotation, head.rotation, step);
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    // Checks for keyboard input
    private void CheckInput()
    {
        // Speed input checks
        CheckMovementInput();
        CalculateSpeed();
    }

    // Update chunks
    private void UpdateChunks()
    {
        if (WorldGen.active != null)
        {
            Vector2Int chunk = Vector2Int.RoundToInt(new Vector2(transform.position.x / 100, transform.position.y / 100));
            if (chunk != chunkCoords) WorldGen.active.UpdateChunks(chunk);
            chunkCoords = chunk;
        }
    }

    // Checks for movement input
    private void CheckMovementInput()
    {
        vertical = 0;
        vertical += Input.GetKey(Keybinds.move_up) ? 1 : 0;
        vertical -= Input.GetKey(Keybinds.move_down) ? 1 : 0;

        horizontal = 0;
        horizontal += Input.GetKey(Keybinds.move_right) ? 1 : 0;
        horizontal -= Input.GetKey(Keybinds.move_left) ? 1 : 0;
    }

    // Calculate speed
    private void CalculateSpeed()
    {
        // Get the stamina stat from player
        Stat stamina = player.GetStat(Stat.Type.Stamina);

        // Check if the player is moving
        bool isMoving = horizontal != 0 || vertical != 0;

        // Check if shift is being held
        if (isMoving && Input.GetKey(KeyCode.LeftShift) && !Tablet.active)
        {
            // Check if player has stamina
            if (stamina.current > 0)
            {
                // Remove stamina from player
                stamina.current -= Time.deltaTime;
                player.SetStat(stamina);

                // Set run speed
                speed = runSpeed;
            }
            else speed = walkSpeed;
        }
        else
        {
            // Update stamina
            if (!stamina.IsAtMax())
            {
                stamina.current += Time.deltaTime * 2f;
                player.SetStat(stamina);
            }

            // Set walk speed or stand still
            if (!isMoving) speed = 0f;
            else speed = walkSpeed;
        }
    }

    // Rotates the players head towards the mouse
    private void RotateToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(model.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        // Rotate head
        head.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float headLocalRotation = head.localRotation.eulerAngles.z;
        if (headLocalRotation > 180) headLocalRotation -= 360;

        // Adjust for overshoot
        if (headLocalRotation >= 30)
        {
            float overShoot = head.localRotation.eulerAngles.z - 30;
            head.localRotation = Quaternion.Euler(new Vector3(0, 0, 30));
            model.Rotate(new Vector3(0, 0, overShoot), Space.World);
        }
        else if (headLocalRotation <= -30)
        {
            float overShoot = head.localRotation.eulerAngles.z + 30;
            head.localRotation = Quaternion.Euler(new Vector3(0, 0, -30));
            model.Rotate(new Vector3(0, 0, overShoot), Space.World);
        }
    }

    // Called by joining clients to set their names
    [Command]
    private void CmdUpdateName(string name)
    {
        playerName = name;
    }

    // On Update Display Name on all clients
    void OnNameChanged(string _Old, string _New)
    {
        nameTag.text = _New;
    }

    // Update player state command
    [Command]
    private void CmdUpdatePlayer(Vector3 position, float bodyRotation, float headRotation, float speed, Vector2 velocity)
    {
        // Todo: Validation
        RpcUpdatePlayer(position, bodyRotation, headRotation, speed, velocity);
    }

    [ClientRpc]
    private void RpcUpdatePlayer(Vector3 position, float bodyRotation, float headRotation, float speed, Vector2 velocity)
    {
        if (hasAuthority) return;

        transform.position = position;
        model.localRotation = Quaternion.Euler(new Vector3(0, 0, bodyRotation));
        head.rotation = Quaternion.Euler(new Vector3(0, 0, headRotation));
        this.speed = speed;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
