using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : ScriptableObject
{
    public new string name; // Name of the resource
    public bool isLiquid; // Determines if biome tile is liquid
    public bool usePerlinNoise; // Tells generator if perlin noise should be used
    public float spawnScale; // Scale of perlin noise
    public float spawnThreshold; // Perlin float threshold
    public float noiseOffset; // Offset perlin noise value
}
