using UnityEngine;

public class Movement : MonoBehaviour
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

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;
    }

    void Update()
    {
        RotateTowardsMouse();

        if (Input.GetKeyDown(KeyCode.Alpha1) && !rightArm.activeSelf) HoldingPistol();
        else if (Input.GetKeyDown(KeyCode.Alpha1)) EmptyHands();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && Input.GetKey(KeyCode.LeftShift)) speed = runSpeed;
        else if (!isMoving) speed = 0f;
        else speed = walkSpeed;

        animator.SetFloat("Speed", speed);
        animator.SetBool("Pistol", rightArm.activeSelf);
    }
    
    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        head.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
