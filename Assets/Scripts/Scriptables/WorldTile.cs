using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile : IdentifiableScriptableObject
{
    public new string name; // Name of the resource
    public bool isLiquid; // Determines if biome tile is liquid
}
