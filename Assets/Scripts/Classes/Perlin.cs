using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Perlin
{
    // Perlin options
    public int octaves = 1;
    [Range(0, 1)] public float persistance = 1;
    public float lacunarity = 0.5f;
    public float scale = 10; 
    public float threshold = 0.8f;
    public Vector2 offset;
}
