using UnityEngine;
using System.Collections;

// Script by Sebastian Lague

public class MapDebug : MonoBehaviour
{
	public Renderer textureRender;

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;
	public float cooldown = 1f;

    public void Update()
    {
		if (autoUpdate)
		{
			cooldown -= Time.deltaTime;
			if (cooldown <= 0f)
			{
				GenerateMap();
				cooldown = 1f;
			}
		}
    }
    public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseChunk(offset, mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity, seed);
		DrawNoiseMap(noiseMap);
	}

	public void DrawNoiseMap(float[,] noiseMap)
	{
		int width = noiseMap.GetLength(0);
		int height = noiseMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
			}
		}
		texture.SetPixels(colourMap);
		texture.Apply();

		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(width, 1, height);
	}

	void OnValidate()
	{
		if (mapWidth < 1)
		{
			mapWidth = 1;
		}
		if (mapHeight < 1)
		{
			mapHeight = 1;
		}
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}
	}

}