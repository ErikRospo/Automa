using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Raw Iron", menuName = "World/Raw Iron")]
public class Resource : ScriptableObject
{
    public new string name;
    public Item item;
    public float spawnScale; // Scale of perlin noise
    public float spawnThreshold; // Perlin float threshold
}
