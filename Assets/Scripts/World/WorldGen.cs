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

    // List of resource tiles
    public ResourceTile resourceTile;
    [HideInInspector] public Dictionary<Vector2Int, MineralData> spawnedResources;
    [HideInInspector] public Grid resourceGrid;

    // List of environment tiles
    public Biome defaultBiome;
    public Tilemap biomeTextureMap;

    // Loaded chunks
    public Dictionary<Vector2Int, Transform> loadedChunks;

    // Debug variables
    public bool enableDebugging;
    public GameObject debugObject;

    public void Start()
    {
        // Get active instance
        active = this;

        // Get world chunk size
        worldChunkSize = chunkSize * 5f;

        // Create a new resource grid
        resourceGrid = new Grid();
        spawnedResources = new Dictionary<Vector2Int, MineralData>();
        resourceGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
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
        int xValue = newChunk.x * chunkSize - chunkOffset;
        int yValue = newChunk.y * chunkSize - chunkOffset;

        // Convert the chunk coordinates to tile position
        Vector2 tilePosition = new Vector2(xValue + chunkOffset, yValue + chunkOffset);

        // Convert the chunk coordinates to world position
        Vector2 worldPosition = new Vector3(tilePosition.x * 5, tilePosition.y * 5, -1);
        GameObject chunk = Instantiate(emptyChunk, worldPosition, Quaternion.identity);
        chunk.name = tilePosition.x + " " + tilePosition.y;

        // If debugging on, visually display show chunk
        if (enableDebugging)
        {
            chunk.GetComponent<MeshRenderer>().enabled = true;
            TextMeshPro text = chunk.GetComponent<TextMeshPro>();
            text.text = chunk.name;
            text.enabled = true;
        }

        // Loop through all biomes and generate it
        foreach(Biome biome in Scriptables.biomes)
        {
            // Check if biome is default
            if (biome.isDefault) continue;

            // Create new noise chunk
            TrySpawnBiome(biome, tilePosition);
        }

        // Iterate through chunk once more, and fill missing tiles with default biome
        TrySpawnBiome(defaultBiome, tilePosition, true);

        return chunk.transform;
    }

    // Try and spawn a biome in a specified chunk
    private void TrySpawnBiome(Biome biome, Vector2 coords, bool fill = false)
    {
        // Create the noise chunk variable
        int sampleSize = chunkSize * 10;
        int chunkOffset = chunkSize / 2;
        float[,] noiseChunk = new float[sampleSize, sampleSize];

        // Create new noise chunk
        if (!fill) noiseChunk = Noise.GenerateNoiseChunk(biome.perlinOptions, coords, sampleSize, seed);

        // Loop through each pixel in the noise chunk
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Coordinates
                int coordX = (int)coords.x + x;
                int coordY = (int)coords.y + y;

                // Default biome
                if (!biomeTextureMap.HasTile(new Vector3Int(coordX, coordY, 0)) &&
                    (fill || noiseChunk[x + chunkOffset, y + chunkOffset] >= biome.perlinOptions.threshold))
                    biomeTextureMap.SetTile(new Vector3Int(coordX, coordY, 0), biome.tile);
            }
        }
    }

    // Try and spawn a resource
    private void TrySpawnResource(MineralData resource, int x, int y, Transform parent)
    {
        // Get x and y pos
        float xPos = x * 5f;
        float yPos = y * 5f;

        // Check cell to make sure it's empty
        if (resourceGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(xPos, yPos))) == null)
        {
            // Create the resource
            ResourceTile temp = Instantiate(resourceTile.gameObject, new Vector3(xPos, yPos, -1), Quaternion.identity).GetComponent<ResourceTile>();
            resourceGrid.SetCell(Vector2Int.RoundToInt(temp.transform.position), true);
            temp.transform.parent = parent;

            // Set resource values
            temp.name = resource.name;
            temp.resource = resource;
            temp.spriteRenderer.sprite = SpritesManager.GetSprite(resource.name);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0f);

            // Add resource to spawned resource list
            spawnedResources.Add(Vector2Int.RoundToInt(temp.transform.position), resource);
        }
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
