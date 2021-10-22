using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/Tile")]
public class Tile : Item
{
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    public GameObject obj;
    public Cell[] cells;
    public Vector2 offset;
    public bool axisSnappable;
    public bool rotatable;
    public bool hasOptions;
    public List<Resource> spawnableOn;
}
