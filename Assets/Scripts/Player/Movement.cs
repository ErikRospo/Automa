using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    public Transform head;

    float horizontal;
    float vertical;

    float previousHeadRotation;
    float previousBodyRotation;

    private float speed;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;

        previousHeadRotation = head.rotation.eulerAngles.z;
        previousBodyRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        RotateTowardsMouse();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && Input.GetKey(KeyCode.LeftShift)) speed = runSpeed;
        else if (!isMoving) speed = 0f;
        else speed = walkSpeed;

        animator.SetFloat("Speed", speed);
    }
    // penis
    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        head.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (head.rotation.eulerAngles.z > 20f) transform.rotation = Quaternion.Euler(new Vector3(0, 0, head.rotation.eulerAngles.z - 20f));
        else if (head.rotation.eulerAngles.z < -20f) transform.rotation = Quaternion.Euler(new Vector3(0, 0, head.rotation.eulerAngles.z + 20f));
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
