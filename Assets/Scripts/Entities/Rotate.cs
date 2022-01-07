using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform obj;
    public float speed;

    // Start is called before the first frame update
    public void Start()
    {
        if (obj == null)
            obj = GetComponent<Transform>();

        RotationHandler.active.RegisterRotator(transform, speed);
        enabled = false;
    }
}
