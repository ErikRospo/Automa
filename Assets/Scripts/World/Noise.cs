using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Sebastian Lague
// Modified by Ben Nichols

public static class Noise
{
	/// <summary>
	/// Generates a noise chunk with the given parameters. (position should be set as tile position)
	/// </summary>
	/// <param name="perlin"></param>
	/// <param name="size"></param>
	/// <param name="position"></param>
	/// <param name="seed"></param>
	/// <returns></returns>
	public static float[,] GenerateNoiseChunk(Perlin perlin, int size, Vector2 position, int seed)
	{
		float[,] noiseMap = new float[size, size];

		Vector2 sampleCoords = new Vector2(
			position.x + perlin.offset.x,
			position.y + perlin.offset.y);

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[perlin.octaves];
		for (int i = 0; i < perlin.octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + sampleCoords.x;
			float offsetY = prng.Next(-100000, 100000) - sampleCoords.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (perlin.scale <= 0)
			perlin.scale = 0.0001f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfSize = size / 2f;

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < perlin.octaves; i++)
				{
					float sampleX = (x - halfSize + octaveOffsets[i].x) / perlin.scale * frequency;
					float sampleY = (y - halfSize + octaveOffsets[i].y) / perlin.scale * frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= perlin.persistance;
					frequency *= perlin.lacunarity;
				}

				if (noiseHeight > maxNoiseHeight)
					maxNoiseHeight = noiseHeight;
				else if (noiseHeight < minNoiseHeight)
					minNoiseHeight = noiseHeight;

				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}
}
