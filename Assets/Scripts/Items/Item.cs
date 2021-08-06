using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : Tile
{
    public new string name;
    public string description;
    public int maxStackSize;

    [System.Serializable]
    public struct Recipe
    {
        public string name;
        public Item item;
        public int amount;
    }
    public Recipe[] recipe;

    /*
    [CreateTileFromPalette]
    public Item CreateTile()
    {
        var tile = ScriptableObject.CreateInstance<Item>();
        tile.name = name;
        tile.description = description;
        tile.maxStackSize = maxStackSize;
        tile.icon = Resources.Load<Sprite>("Icons/" + name);
        return tile;
    }
    */
}
