using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
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
}
