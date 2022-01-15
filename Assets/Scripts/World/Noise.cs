using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Sebastian Lague

public static class Noise
{
    /// <summary>
    /// Generates and returns a noise chunk with multiple perlin samples
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
    public static float[,] GenerateNoiseChunk(Perlin perlinOptions, int xValue, int yValue, int sampleSize, int seed, bool localize = false)
    {
        // Create new multi-dimensional array for the noise chunk
        float[,] noiseChunk = new float[sampleSize, sampleSize];

        // Create a new random generator with the specified seed
        System.Random random = new System.Random(seed);

        // Array for number of specified octaves
        Vector2[] octaveOffsets = new Vector2[perlinOptions.octaves];

        // Loop through octaves to generate random offsets with specified seed
        for (int i = 0; i < perlinOptions.octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + perlinOptions.offset;
            float offsetY = random.Next(-100000, 100000) + perlinOptions.offset;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Create variables to hold bounds of the noise chunk
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Iterate through noise chunk
        for (int x = 0; x < sampleSize; x++)
        {
            for (int y = 0; y < sampleSize; y++)
            {
                // Internal loop values
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Sample multiple noise passes
                for (int i = 0; i < perlinOptions.octaves; i++)
                {
                    float sampleX = (x + xValue) / perlinOptions.scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y + yValue) / perlinOptions.scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    if (perlinOptions.useMultiSampling)
                    {
                        amplitude *= perlinOptions.persistance;
                        frequency *= perlinOptions.lacunarity;
                    }
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
        for (int x = 0; x < sampleSize; x++)
        {
            for (int y = 0; y < sampleSize; y++)
            {
                if (localize)
                {
                    noiseChunk[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseChunk[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseChunk[x, y] + 1) / (maxNoiseHeight / 0.9f);
                    noiseChunk[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
            }
        }

        // Return the newly generated noise chunk
        return noiseChunk;
    }
}
