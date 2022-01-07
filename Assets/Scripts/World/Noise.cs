using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Sebastian Lague

public static class Noise
{
    /// <summary>
    /// Generates and returns a noise chunk with multiple passes.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="scale"></param>
    /// <param name="octaves"></param>
    /// <param name="persistance"></param>
    /// <param name="lacunarity"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static float[,] GenerateNoiseChunk(PerlinOptions perlinOptions, Vector2 offset, int size, int seed)
    {
        // Create new multi-dimensional array for the noise chunk
        float[,] noiseChunk = new float[size, size];

        // Create a new random generator with the specified seed
        System.Random random = new System.Random(seed);

        // Array for number of specified octaves
        Vector2[] octaveOffsets = new Vector2[perlinOptions.octaves];

        // Loop through octaves to generate random offsets with specified seed
        for (int i = 0; i < perlinOptions.octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Create variables to hold bounds of the noise chunk
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Divide width and height in half
        float halfSize = size / 2f;

        // Iterate through noise chunk
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Internal loop values
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Sample multiple noise passes
                for (int i = 0; i < perlinOptions.octaves; i++)
                {
                    float sampleX = (x - halfSize) / perlinOptions.scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfSize) / perlinOptions.scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= perlinOptions.persistance;
                    frequency *= perlinOptions.lacunarity;
                }

                // Assign bounds of lowest and highest noise pixel
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                // Assign noise chunk
                noiseChunk[x, y] = noiseHeight;
            }
        }

        // Iterate through bounds of the noise chunk and lerp
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                noiseChunk[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseChunk[x, y]);
            }
        }

        // Return the newly generated noise chunk
        return noiseChunk;
    }
}
