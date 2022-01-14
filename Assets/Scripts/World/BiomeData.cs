using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "World/Biome")]
public class BiomeData : WorldTile
{
    public TileBase tile; // Biome tile asset
    public bool isDefault;
}
