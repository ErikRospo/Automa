using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerlinOptions
{
    // Parameterized constructor
    public PerlinOptions(int scale = 0, int threshold = 0, int offset = 0,
        int octaves = 0, int persistance = 0, int lacunarity = 0)
    {
        this.scale = scale;
        this.threshold = threshold;
        this.offset = offset;
        this.octaves = octaves;
        this.persistance = persistance;
        this.lacunarity = lacunarity;
    }

    // Perlin options
    public int octaves = 1;
    public float persistance = 1;
    [Range(0, 1)] public float lacunarity = 0.5f;
    public float scale = 10; 
    public float threshold = 0; 
    public float offset = 0; 
}
