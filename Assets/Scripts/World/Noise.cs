using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseChunk(int chunkWidth, int chunkHeight, float noiseScale)
    {
        // Generate new noise array 
        float[,] chunk = new float[chunkWidth, chunkHeight];

        // Check noise scale 
        if (noiseScale <= 0) noiseScale = 0.0001f; 

        // Loop through pixels and set to chunk
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                float xCoord = x / noiseScale;
                float yCoord = y / noiseScale;

                float value = Mathf.PerlinNoise(xCoord, yCoord);
                chunk[x, y] = value;
            }
        }

        return chunk;
    }
}
