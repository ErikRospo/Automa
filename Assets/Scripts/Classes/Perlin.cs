using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Perlin
{
    // Perlin options
    public int octaves = 4;
    public float lacunarity = 2f;
    [Range(0, 1)] public float persistance = 0.5f;
    [Range(0, 2)] public float modifier = 1.5f;
    public float scale = 10; 
    public float threshold = 0.8f;
    public int seedOffset;
    public Vector2 sampleOffset;
}
