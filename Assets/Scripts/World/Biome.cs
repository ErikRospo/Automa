using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "World/Biome")]
public class Biome : WorldTile
{
    public TileBase tile; // Biome tile asset
}
