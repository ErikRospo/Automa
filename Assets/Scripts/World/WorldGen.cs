using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    // Active instance
    public static WorldGen active;

    // List of resource tiles
    public ResourceTile resourceTile;
    public List<Resource> resources;
    public float offset = 0;

    // Resource grid
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

            // Loop through resources
            foreach (Resource resource in resources)
            {
                // Get world coordinate
                int xValue = newChunk.x * 100;
                int yValue = newChunk.y * 100;

                for (int x = xValue; x < xValue + 100; x++)
                {
                    for (int y = yValue; y < yValue + 100; y++)
                    {
                        // Calculate perlin noise pixel
                        float xCoord = ((float)x / 100 * resource.spawnScale) + offset;
                        float yCoord = ((float)y / 100 * resource.spawnScale) + offset;
                        float value = Mathf.PerlinNoise(xCoord, yCoord);

                        // If value exceeds threshold, try and generate
                        if (value >= resource.spawnThreshold)
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
                    }
                }
            }
        }

        loadedChunks = new List<Vector2Int>(chunks);
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
