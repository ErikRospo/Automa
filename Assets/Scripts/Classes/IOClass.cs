using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IOClass
{ 
    public Transform transform;
    public Transform tile;

    public Entity bin;
    [HideInInspector] public int binAmount;
    public Building target;
    public bool reserved;

    public Vector3 position;
    public Vector3 tilePosition;
}
