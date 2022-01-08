using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script adapted from "Pretty Fly Games" https://www.youtube.com/watch?v=DVHcOS1E5OQ

public class CarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelertationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;

    float velocityVsUp = 0;

    Rigidbody2D carRigidbody;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0) return;

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) return;

        if (carRigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;

        if (accelerationInput == 0)
            carRigidbody.drag = Mathf.Lerp(carRigidbody.drag, 3.0f, Time.fixedDeltaTime * 3);
        else
            carRigidbody.drag = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelertationFactor;

        carRigidbody.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        float minSpeed = (carRigidbody.velocity.magnitude / 8);
        minSpeed = Mathf.Clamp01(minSpeed);

        rotationAngle += steeringInput * turnFactor * minSpeed;

        carRigidbody.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody.velocity, transform.right);

        carRigidbody.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x * -1;
        accelerationInput = inputVector.y;
    }
}
