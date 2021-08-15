using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public new string name;
    public string description;
    public int maxStackSize;
    public GameObject obj;

    [System.Serializable]
    public struct RecipeItem
    {
        public string name;
        public Item item;
        public int amount;
    }

    [System.Serializable]
    public struct Recipes
    {
        public string name;
        public RecipeItem[] items;
        public bool unlocked;
    }

    public Recipes[] recipes;

    
}
