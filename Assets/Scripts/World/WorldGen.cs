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
        int xValue = newChunk.x * chunkSize + chunkOffset;
        int yValue = newChunk.y * chunkSize + chunkOffset;

        // Create new chunk parent
        GameObject chunk = Instantiate(emptyChunk, new Vector3((xValue * 5) + (chunkOffset * 5), 
            (yValue * 5) + (chunkOffset * 5), -1), Quaternion.identity);
        chunk.name = "Chunk " + newChunk;
        
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
            // Create new noise chunk
            // float[,] noiseChunk = Noise.GenerateNoiseChunk(newChunk, chunkSize, chunkSize, biome.spawnScale, biome.octaves, biome.persistance, biome.lacunarity, seed);

            // Loop through each pixel in the noise chunk
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    // Get world position of noise map
                    Vector2 worldPos = new Vector2((xValue + x) * 5, (yValue + y) * 5);

                    // Default biome
                    if (!biomeTextureMap.HasTile(new Vector3Int(x + xValue, y + yValue, 0)))
                    {
                        biomeTextureMap.SetTile(new Vector3Int(x + xValue, y + yValue, 0), defaultBiome.tile);
                    }
                }
            }
        }

        return chunk.transform;
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
                chunks.Add(new Vector2Int(x, y));

        // Return chunk coordinates
        return chunks;
    }
}
