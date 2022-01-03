using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    // Active instance
    public static WorldGen active;

    // Chunk variables
    public GameObject emptyChunk;
    public int renderDistance = 5;
    public int chunkSize = 20;
    public float perlinScale = 500;

    // List of resource tiles
    public ResourceTile resourceTile;
    [HideInInspector] public Dictionary<Vector2Int, MineralData> spawnedResources;

    // List of environment tiles
    public Biome defaultBiome;
    public Tilemap biomeTextureMap;

    // Resource grid
    [HideInInspector] public Grid resourceGrid;

    // Loaded chunks
    public Dictionary<Vector2Int, Transform> loadedChunks;

    public void Start()
    {
        // Get active instance
        active = this;

        // Create a new resource grid
        resourceGrid = new Grid();
        spawnedResources = new Dictionary<Vector2Int, MineralData>();
        resourceGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
        loadedChunks = new Dictionary<Vector2Int, Transform>();

        // Set random seed
        #pragma warning disable CS0618
        Random.seed = Random.Range(0, 1000000000);
        #pragma warning restore CS0618
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

        // Flag for default biome
        Biome lastTile = defaultBiome;
        bool useDefaultBiome;

        // Create chunk parent
        GameObject chunk = Instantiate(emptyChunk, new Vector3(xValue * 5, yValue * 5, -1), Quaternion.identity);
        chunk.name = "Chunk " + newChunk;

        // Create min and max values
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Get map noise
        

        // Loop through x and y coordinates
        for (int x = xValue - chunkOffset; x < xValue + chunkOffset; x++)
        {
            for (int y = yValue - chunkOffset; y < yValue + chunkOffset; y++)
            {
                // Default biome
                useDefaultBiome = true;

                // Generate noise map
                foreach (Biome biome in Scriptables.biomes)
                {
                    // Create offset variables to track perlin passes
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 1;

                    // Use perlin passes to give natural looking terrain
                    for (int i = 0; i < biome.octaves; i++)
                    {
                        // Calculate perlin noise pixel
                        float xCoord = (((float)x / biome.spawnScale) + biome.noiseOffset) * frequency;
                        float yCoord = (((float)y / biome.spawnScale) + biome.noiseOffset) * frequency;
                        float value = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                        noiseHeight += value * amplitude;
                        amplitude *= biome.persistance;
                        frequency *= biome.lacunarity;
                    }

                    // Set min and max values
                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    // If value exceeds threshold, try and generate
                    if (noiseHeight >= biome.spawnThreshold)
                    {
                        biomeTextureMap.SetTile(new Vector3Int(x, y, 0), biome.tile);
                        useDefaultBiome = false;
                        lastTile = biome;
                        break;
                    }
                }

                if (useDefaultBiome) biomeTextureMap.SetTile(new Vector3Int(x, y, 0), defaultBiome.tile);

                // Loop through resources
                foreach (MineralData mineral in Scriptables.minerals)
                {
                    // Check if resource can spawn on tile
                    if (lastTile.isLiquid && !mineral.canSpawnOnLiquid) continue;
                    else if (!lastTile.isLiquid && !mineral.canSpawnOnLand) continue;

                    // Check if resource uses perlin noise
                    if (!mineral.usePerlinNoise)
                    {
                        // Calculate random float value
                        float value = Random.value;
                        if (mineral.spawnThreshold < value)
                        {
                            // If threshold exceed value, spawn
                            TrySpawnResource(mineral, x, y, chunk.transform);
                            break;
                        }
                    }
                    else
                    {
                        // Calculate perlin noise pixel
                        float xCoord = ((float)x / mineral.spawnScale) + mineral.noiseOffset;
                        float yCoord = ((float)y / mineral.spawnScale) + mineral.noiseOffset;
                        float value = Mathf.PerlinNoise(xCoord, yCoord);

                        // If value exceeds threshold, try and generate
                        if (value >= mineral.spawnThreshold)
                        {
                            TrySpawnResource(mineral, x, y, chunk.transform);
                            break;
                        }
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
        for (int x = chunk.x - renderDistance; x < chunk.x + renderDistance; x++)
            for (int y = chunk.y - renderDistance; y < chunk.y + renderDistance; y++)
                chunks.Add(new Vector2Int(x, y));

        // Return chunk coordinates
        return chunks;
    }
}
