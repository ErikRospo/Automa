using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IOClass
{ 
    public Transform transform;
    public Transform tile;

    [HideInInspector] public Entity bin;
    [HideInInspector] public int binAmount;
    public Building target;
    [HideInInspector] public bool reserved;

    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 tilePosition;
}
