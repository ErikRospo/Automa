using Mirror;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    Rigidbody2D body;
    Animator animator;

    public Transform head;
    public GameObject rightArm;
    public GameObject rightHand;

    float horizontal;
    float vertical;

    private float speed;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 5f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    [Client]
    void Update()
    {
        if (!hasAuthority) return;

        RotateTowardsMouse();

        if (Input.GetKeyDown(KeyCode.Alpha1) && !rightArm.activeSelf) HoldingPistol();
        else if (Input.GetKeyDown(KeyCode.Alpha1)) EmptyHands();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && Input.GetKey(KeyCode.LeftShift)) speed = runSpeed;
        else if (!isMoving) speed = 0f;
        else speed = walkSpeed;
        CmdUpdatePlayer(transform);

        animator.SetFloat("Speed", speed);
        animator.SetBool("Pistol", rightArm.activeSelf);
    }

    // Update player state command
    [Command]
    private void CmdUpdatePlayer(Transform pos)
    {
        // Todo: Movement Validation

        RpcUpdatePlayer(pos);
    }

    [ClientRpc]
    private void RpcUpdatePlayer(Transform pos)
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;
    }
    
    private void RotateTowardsMouse()
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

    private void HoldingPistol()
    {
        rightArm.SetActive(true);
        rightHand.SetActive(false);
    }

    private void EmptyHands()
    {
        rightArm.SetActive(false);
        rightHand.SetActive(true);
    }


    private void FixedUpdate()
    {
        var step = rotationSpeed * Time.deltaTime;
        if (speed > 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, head.rotation, step);
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
