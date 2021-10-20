using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "World/Biome")]
public class BiomeTile : ScriptableObject
{
    public new string name; // Name of the resource
    public TileBase tile; // Biome tile asset
    public bool isLiquid; // Determines if biome tile is liquid
    public float spawnScale; // Scale of perlin noise
    public float spawnThreshold; // Perlin float threshold
    public float noiseOffset; // Offset perlin noise 
}
