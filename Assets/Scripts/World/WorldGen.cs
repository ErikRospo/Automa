using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class WorldGen : MonoBehaviour
{
    // Chunk class


    // Active instance
    public static WorldGen active;

    // Point to the active viewer
    public Transform viewer;
    public static Vector2 viewerPosition;

    // Chunk variables
    public int seed;
    public int chunkSize = 10;
    private int viewDistance = 8;
    public GameObject emptyChunk;

    // List of environment tiles
    public BiomeData defaultBiome;
    public Tilemap biomeMap;
    public Tilemap resourceMap;
    public Dictionary<Vector2, DepositData> resources;

    // Loaded chunks
    public Dictionary<Vector2Int, GameObject> loadedChunks;

    // Debug variables
    public bool enableChunkDebugging;
    public bool enableNoiseDebugging;
    public GameObject debugNoiseText;
    public WorldTile debugNoiseType;

    public void Start()
    {
        // Get active instance
        active = this;

        // Create a new resource grid
        loadedChunks = new Dictionary<Vector2Int, GameObject>();

        // On player spawn event
        Events.active.onPlayerSpawned += SetViewer;
    }

    public void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.y);
        UpdateChunks();
    }

    // Set the active viewer
    public void SetViewer(Player player)
    {
        if (player.hasAuthority)
            viewer = player.transform;
    }

    // Generate resources
    public void UpdateChunks()
    {
        // Get chunk coordinates
        Vector2Int coords = Vector2Int.RoundToInt(new Vector2(
            viewerPosition.x / chunkSize,
            viewerPosition.y / chunkSize));

        // Create new chunks list
        List<Vector2Int> chunks = GetChunks(coords);
        List<GameObject> chunksLoaded = new List<GameObject>();

        // Loops through chunks
        foreach (Vector2Int chunkCoords in chunks)
        {
            // Check that the chunk isn't loaded
            if (loadedChunks.ContainsKey(chunkCoords))
            {
                loadedChunks[chunkCoords].SetActive(true);
                chunksLoaded.Add(loadedChunks[chunkCoords]);
            }
            else
            {
                GameObject newChunk = GenerateNewChunk(chunkCoords);
                loadedChunks.Add(chunkCoords, newChunk);
                chunksLoaded.Add(newChunk);
            }
        }

        // Disable chunks that are out of sight
        foreach (GameObject loadedChunk in loadedChunks.Values)
            if (!chunksLoaded.Contains(loadedChunk))
                loadedChunk.gameObject.SetActive(false);
    }

    // Loops through a new chunk and spawns resources based on perlin noise values
    private GameObject GenerateNewChunk(Vector2Int newChunk)
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

        return chunk;
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
        for (int x = chunk.x - viewDistance; x < chunk.x + viewDistance; x++)
            for (int y = chunk.y - viewDistance; y < chunk.y + viewDistance; y++)
                chunks.Add(new Vector2Int(x, y));

        // Return chunk coordinates
        return chunks;
    }
}
