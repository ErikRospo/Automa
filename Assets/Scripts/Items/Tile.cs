using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Parent Building", menuName = "Items/Tile")]
public class Tile : Item
{
    [System.Serializable]
    public struct Cell
    {
        public int x;
        public int y;
    }

    public GameObject obj;
    public Cell[] cells;
}
