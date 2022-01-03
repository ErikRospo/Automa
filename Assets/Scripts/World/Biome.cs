using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "World/Biome")]
public class Biome : WorldTile
{
    public TileBase tile; // Biome tile asset
    public int octaves = 0;
    public float persistance = 0;
    public float lacunarity = 0;
}
