using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    // Active instance
    public static WorldGen active;

    // List of resource tiles
    public ResourceTile resourceTile;
    public List<Resource> resources;
    public float offset = 0;
    public float perlinScale = 500;

    // Resource grid
    public Tilemap biomeTextureMap;
    [HideInInspector] public Grid biomeGrid;
    [HideInInspector] public Grid resourceGrid;

    // Loaded chunks
    public List<Vector2Int> loadedChunks;

    public void Start()
    {
        // Get active instance
        active = this;

        // Create a new resource grid
        resourceGrid = new Grid();
        resourceGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();

        // Set random seed
        Random.InitState(Random.Range(0, 1000000000));
        if (offset == 0) offset = Random.Range(0, 10000);
    }
    
    // Generate resources
    public void UpdateChunks(Vector2Int chunk)
    {
        // Create new chunks list
        List<Vector2Int> chunks = GetChunks(chunk);

        // Loops through chunks
        foreach (Vector2Int newChunk in chunks)
        {
            // Check that the chunk isn't loaded
            if (loadedChunks.Contains(newChunk)) continue;

            // A CHECK NEEDS TO BE DONE HERE
            // If chunk is brand new, generate biome / resources
            // If chunk is not new, grab it from the object pool 
            // If chunk is outside of player view, pool and disable it

            // Update biome
            //UpdateBiome(newChunk);
            GenerateResources(newChunk);
        }

        // Update loaded chunks
        loadedChunks = new List<Vector2Int>(chunks);
    }

    // Loops through a chunk and updates the biome if required
    private void UpdateBiome(Vector2Int newChunk)
    {

    }

    // Loops through a new chunk and spawns resources based on perlin noise values
    private void GenerateResources(Vector2Int newChunk)
    {
        // Get world coordinate
        int xValue = newChunk.x * 20;
        int yValue = newChunk.y * 20;

        // Loop through x and y coordinates
        for (int x = xValue; x < xValue + 100; x++)
        {
            for (int y = yValue; y < yValue + 100; y++)
            {
                // Loop through resources
                foreach (Resource resource in resources)
                {
                    // ANOTHER CHECK NEEDS TO BE DONE HERE
                    // - Check if resource being spawned is able to reside in biome
                    // - If not, skip. If so, spawn it.

                    // Check if resource uses perlin noise
                    if (!resource.usePerlinNoise)
                    {
                        // Calculate random float value
                        float value = Random.value;
                        if (resource.spawnThreshold < value)
                        {
                            // If threshold exceed value, spawn
                            TrySpawnResource(resource, x, y);
                            break;
                        }
                    }
                    else
                    {
                        // Calculate perlin noise pixel
                        float xCoord = ((float)x / perlinScale) + offset;
                        float yCoord = ((float)y / perlinScale) + offset;
                        float value = Mathf.PerlinNoise(xCoord, yCoord);

                        // If value exceeds threshold, try and generate
                        if (value >= resource.spawnThreshold)
                        {
                            TrySpawnResource(resource, x, y);
                            break;
                        }
                    }
                }
            }
        }
    }

    // Try and spawn a resource
    private void TrySpawnResource(Resource resource, int x, int y)
    {
        // Get x and y pos
        float xPos = (x * 5f) - 250;
        float yPos = (y * 5f) - 250;

        // Check cell to make sure it's empty
        if (resourceGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(xPos, yPos))) == null)
        {
            // Create the resource
            ResourceTile temp = Instantiate(resourceTile.gameObject, new Vector3(xPos, yPos, -1), Quaternion.identity).GetComponent<ResourceTile>();
            resourceGrid.SetCell(Vector2Int.RoundToInt(temp.transform.position), true);

            // Set resource values
            temp.name = resource.name;
            temp.resource = resource;
            temp.spriteRenderer.sprite = SpritesManager.GetSprite(resource.name);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0f);
        }
    }

    private List<Vector2Int> GetChunks(Vector2Int chunk)
    {
        // Create new chunks list
        List<Vector2Int> chunks = new List<Vector2Int>();

        // Add chunk to chunks list
        chunks.Add(chunk);

        // Grab chunks on top
        chunks.Add(new Vector2Int(chunk.x - 1, chunk.y + 1));
        chunks.Add(new Vector2Int(chunk.x, chunk.y + 1));
        chunks.Add(new Vector2Int(chunk.x + 1, chunk.y + 1));

        // Grab side chunks
        chunks.Add(new Vector2Int(chunk.x + 1, chunk.y));
        chunks.Add(new Vector2Int(chunk.x - 1, chunk.y));

        // Grab chunks on bottom
        chunks.Add(new Vector2Int(chunk.x - 1, chunk.y - 1));
        chunks.Add(new Vector2Int(chunk.x, chunk.y - 1));
        chunks.Add(new Vector2Int(chunk.x + 1, chunk.y - 1));

        return chunks;
    }
}
