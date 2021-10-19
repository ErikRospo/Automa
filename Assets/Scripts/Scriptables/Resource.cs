using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "World/Resource")]
public class Resource : ScriptableObject
{
    public new string name; // Name of the resource
    public Item item; // Item that will be given when mined
    public bool usePerlinNoise; // Tells generator if perlin noise should be used
    public bool randomizeRotation; // Tells generator if resource can be rotated
    public float spawnScale; // Scale of perlin noise
    public float spawnThreshold; // Perlin float threshold
}
