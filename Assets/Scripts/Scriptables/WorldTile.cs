using UnityEngine;

public class WorldTile : IdentifiableScriptableObject
{
    public new string name; // Name of the resource
    public bool isLiquid; // Determines if biome tile is liquid
    public bool usePerlinNoise; // Tells generator if perlin noise should be used
    public PerlinOptions perlinOptions; // Options to modify perlin noise
}
