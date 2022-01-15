using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class WorldGen : MonoBehaviour
{
    // Active instance
    public static WorldGen active;

    // Chunk variables
    public int seed;
    public GameObject emptyChunk;
    public int chunkSize = 10;
    private int horizontalRenderDistance = 7;
    private int verticalRenderDistance = 4;
    [HideInInspector] public float worldChunkSize;

    // List of environment tiles
    public BiomeData defaultBiome;
    public Tilemap biomeMap;
    public Tilemap resourceMap;
    public Dictionary<Vector2, DepositData> resources;

    // Loaded chunks
    public Dictionary<Vector2Int, Transform> loadedChunks;

    // Debug variables
    public bool enableChunkDebugging;
    public bool enableNoiseDebugging;
    public GameObject debugNoiseText;
    public WorldTile debugNoiseType;

    public void Start()
    {
        // Get active instance
        active = this;

        // Get world chunk size
        worldChunkSize = chunkSize * 5f;

        // Create a new resource grid
        loadedChunks = new Dictionary<Vector2Int, Transform>();
    }
    
    // Generate resources
    public void UpdateChunks(Vector2Int chunk)
    {
        // Create new chunks list
        List<Vector2Int> chunks = GetChunks(chunk);
        List<Transform> chunksLoaded = new List<Transform>();

        // Loops through chunks
        foreach (Vector2Int chunkCoords in chunks)
        {
            // Check that the chunk isn't loaded
            if (loadedChunks.ContainsKey(chunkCoords))
            {
                loadedChunks[chunkCoords].gameObject.SetActive(true);
                chunksLoaded.Add(loadedChunks[chunkCoords]);
            }
            else
            {
                Transform newChunk = GenerateNewChunk(chunkCoords);
                loadedChunks.Add(chunkCoords, newChunk);
                chunksLoaded.Add(newChunk);
            }
        }

        // Disable chunks that are out of sight
        foreach (Transform loadedChunk in loadedChunks.Values)
            if (!chunksLoaded.Contains(loadedChunk))
                loadedChunk.gameObject.SetActive(false);
    }

    // Loops through a new chunk and spawns resources based on perlin noise values
    private Transform GenerateNewChunk(Vector2Int newChunk)
    {
        // Get middle offset
        int chunkOffset = chunkSize / 2;

        // Get world coordinate
        int xValue = (newChunk.x * chunkSize) - chunkOffset;
        int yValue = (newChunk.y * chunkSize) - chunkOffset;

        // Convert the chunk coordinates to tile position
        Vector2Int tilePosition = new Vector2Int(xValue + chunkOffset, yValue + chunkOffset);

        // Convert the chunk coordinates to world position
        Vector2 worldPosition = new Vector2(tilePosition.x * 5, tilePosition.y * 5);
        GameObject chunk = Instantiate(emptyChunk, worldPosition, Quaternion.identity);
        chunk.name = tilePosition.x + " " + tilePosition.y;

        // If debugging on, visually display show chunk
        if (enableChunkDebugging)
        {
            chunk.GetComponent<MeshRenderer>().enabled = true;
            TextMeshPro text = chunk.GetComponent<TextMeshPro>();
            text.text = chunk.name;
            text.enabled = true;
        }

        // Loop through all biomes and generate it
        foreach(BiomeData biome in Scriptables.biomes)
        {
            // Check if biome is default
            if (biome.isDefault) continue;

            // Create new noise chunk
            TrySpawnBiome(biome, tilePosition);
        }

        // Loop through all resources and generate it
        foreach(DepositData deposit in Scriptables.deposits)
        {
            // Create new noise chunk and spawn
            TrySpawnResource(deposit, tilePosition);
        }

        // Iterate through chunk once more, and fill missing tiles with default biome
        TrySpawnBiome(defaultBiome, tilePosition, true);

        return chunk.transform;
    }

    // Try and spawn a biome in a specified chunk
    private void TrySpawnBiome(BiomeData biome, Vector2Int coords, bool fill = false)
    {
        
    }

    // Try and spawn a biome in a specified chunk
    private void TrySpawnResource(DepositData resource, Vector2Int coords)
    {
        
    }

    private List<Vector2Int> GetChunks(Vector2Int chunk)
    {
        // Create new chunks list
        List<Vector2Int> chunks = new List<Vector2Int>();

        // Get surrounding chunks based on render distance
        for (int x = chunk.x - horizontalRenderDistance; x < chunk.x + horizontalRenderDistance; x++)
            for (int y = chunk.y - verticalRenderDistance; y < chunk.y + verticalRenderDistance; y++)
                chunks.Add(new Vector2Int(x - 1, y - 1));

        // Return chunk coordinates
        return chunks;
    }
}
