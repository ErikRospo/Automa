using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : ScriptableObject
{
    public new string name; // Name of the resource
    public bool usePerlinNoise; // Tells generator if perlin noise should be used
    public bool randomizeRotation; // Tells generator if resource can be rotated
    public bool canSpawnOnLand; // Tells generator if resource can spawn on land
    public bool canSpawnOnLiquid; // Tells generator if resource can spawn on liquid
    public float spawnScale; // Scale of perlin noise
    public float spawnThreshold; // Perlin float threshold
    public float noiseOffset; // Offset perlin noise value
}
