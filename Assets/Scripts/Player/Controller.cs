using Mirror;
using UnityEngine;

// Handles anything player-input related

public class Controller : NetworkBehaviour
{
    // Object animator 
    Animator animator;

    // Inventory script (attached to object)
    Inventory inventory;

    // GameObject child transforms
    private Rigidbody2D body;
    public Transform head;
    public Transform itemContainer;

    // Holds the equipped item
    public Item equippedItem;

    // Movement variables
    float horizontal;
    float vertical;

    // Player movement variables
    private float speed;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 5f;

    // Called on start
    void Start()
    {
        // Start for everyone
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        
        // Initialize walk speed
        speed = walkSpeed;

        // Start for owner only
        if (!hasAuthority) return;
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    // Normal frame update
    [Client]
    void Update()
    {
        // Update for Everyone
        animator.SetFloat("Speed", speed);

        // Upate for owner
        if (!hasAuthority) return;

        // Rotates player body to mouse
        RotateToMouse();

        // Checks for keyboard input
        CheckInput();

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && Input.GetKey(KeyCode.LeftShift)) speed = runSpeed;
        else if (!isMoving) speed = 0f;
        else speed = walkSpeed;

        CmdUpdatePlayer(transform.position, transform.rotation.eulerAngles.z, head.rotation.eulerAngles.z, speed);
    }

    // Physics update for handling movement calculations 
    private void FixedUpdate()
    {
        if (!hasAuthority) return;

        var step = rotationSpeed * Time.deltaTime;
        if (speed > 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, head.rotation, step);
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    // Checks for keyboard input
    private void CheckInput()
    {
        vertical = 0;
        vertical += Input.GetKey(Keybinds.move_up) ? 1 : 0;
        vertical -= Input.GetKey(Keybinds.move_down) ? 1 : 0;

        horizontal = 0;
        horizontal += Input.GetKey(Keybinds.move_right) ? 1 : 0;
        horizontal -= Input.GetKey(Keybinds.move_left) ? 1 : 0;
    }

    // Rotates the players head towards the mouse
    private void RotateToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

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
            transform.Rotate(new Vector3(0, 0, overShoot), Space.World);
        }
        else if (headLocalRotation <= -30)
        {
            float overShoot = head.localRotation.eulerAngles.z + 30;
            head.localRotation = Quaternion.Euler(new Vector3(0, 0, -30));
            transform.Rotate(new Vector3(0, 0, overShoot), Space.World);
        }
    }

    // Update player state command
    [Command]
    private void CmdUpdatePlayer(Vector3 position, float bodyRotation, float headRotation, float speed)
    {
        // Todo: Validation
        RpcUpdatePlayer(position, bodyRotation, headRotation, speed, false);
    }

    [ClientRpc]
    private void RpcUpdatePlayer(Vector3 position, float bodyRotation, float headRotation, float speed, bool force)
    {
        if (!force && hasAuthority) return;

        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, bodyRotation));
        head.rotation = Quaternion.Euler(new Vector3(0, 0, headRotation));
        this.speed = speed;
    }
}
