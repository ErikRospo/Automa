﻿using Mirror;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    Rigidbody2D body;
    Animator animator;

    public Transform head;
    public GameObject pistol;

    bool holdingPistol = false;

    float horizontal;
    float vertical;

    private float speed;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 5f;

    void Start()
    {
        // Start for everyone
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = walkSpeed;

        // Start for owner only
        if (!hasAuthority) return;
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    [Client]
    void Update()
    {
        // Update for Everyone
        animator.SetFloat("Speed", speed);

        // Upate for owner
        if (!hasAuthority) return;

        RotateTowardsMouse();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            holdingPistol = !holdingPistol;
            pistol.SetActive(holdingPistol);
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;

        if (isMoving && Input.GetKey(KeyCode.LeftShift) && !holdingPistol) speed = runSpeed;
        else if (!isMoving) speed = 0f;
        else speed = walkSpeed;

        
        animator.SetBool("Pistol", holdingPistol);

        CmdUpdatePlayer(transform.position, transform.rotation.eulerAngles.z, head.rotation.eulerAngles.z, speed);
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

    private void FixedUpdate()
    {
        if (!hasAuthority) return;

        var step = rotationSpeed * Time.deltaTime;
        if (speed > 0) transform.rotation = Quaternion.RotateTowards(transform.rotation, head.rotation, step);
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
