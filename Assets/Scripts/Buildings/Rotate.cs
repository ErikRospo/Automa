using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    public void Start()
    {
        RotationHandler.active.RegisterRotator(transform, speed);
        enabled = false;
    }
}
