using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Perlin
{
    // Layering class
    [System.Serializable]
    public class Threshold
    {
        public Tile tile;
        public float minThreshold, maxThreshold;
        //public int minAmount, maxAmount;
        //public bool randomizeAmounts;
    }

    // Perlin options
    public bool useMultiSampling = false;
    public int octaves = 1;
    public float persistance = 1;
    [Range(0, 1)] public float lacunarity = 0.5f;
    public float scale = 10; 
    public float offset = 0;
    public float threshold = 0.8f;
}
