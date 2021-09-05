using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outputs
{
    public Building target;

    public Transform transform;
    public Transform tile;
    public Entity bin;

    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 tilePosition;
    [HideInInspector] public bool reserved;
}
