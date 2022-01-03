using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/Building")]
public class BuildingData : ItemData
{
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    public GameObject obj;
    public Cell[] cells;
    public Vector2 horizontalOffset;
    public Vector2 verticalOffset;
    public bool axisSnappable;
    public bool rotatable;
    public bool hasOptions;
    public List<MineralData> spawnableOn;
}