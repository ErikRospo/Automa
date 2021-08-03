using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Screen.fullScreen = false;
    }

    void FixedUpdate()
    {
        // Check if target is null
        if (target == null) return;

        Vector3 goalPos = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }

    public void SetTarget(Transform target)
    {
        // Todo: Validate target is a targetable target
        this.target = target;
    }
}

