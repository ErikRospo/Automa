using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile : IdentifiableScriptableObject
{
    public new string name; // Name of the resource
    public bool isLiquid; // Determines if biome tile is liquid
    public bool usePerlinNoise; // Tells generator if perlin noise should be used
    public Perlin perlinOptions; // Options to modify perlin noise

    /*
    /// <summary>
    /// Returns the minimum threshold for this tile to spawn
    /// </summary>
    /// <returns></returns>
    public float GetMinimumThreshold()
    {
        if (perlinOptions.threshold.Count == 0)
        {
            Debug.Log("Tile needs at least one perlin threshold defined!");
            return 0;
        }
        else return perlinOptions.threshold[0].minThreshold;
    }

    /// <summary>
    /// Returns a tile using a noise value
    /// </summary>
    /// <param name="noise"></param>
    /// <returns></returns>
    public Tile GetTile(float noise)
    {
        foreach(Perlin.Threshold threshold in perlinOptions.threshold)
        {
            if (threshold.maxThreshold >= noise && threshold.minThreshold <= noise)
            {
                return threshold.tile;
            }
        }

        return null;
    }
    */

}
