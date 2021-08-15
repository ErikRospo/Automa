using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [System.Serializable]
    public struct RecipeItem
    {
        public string name;
        public Item item;
        public int amount;
    }

    [System.Serializable]
    public struct SmelterRecipe
    {
        public string name;
        public RecipeItem input;
        public RecipeItem output;
        public int time;
        public bool unlocked;
    }

    public SmelterRecipe[] smelterRecipes;
    public static SmelterRecipe[] _smelterRecipes;

    private void Start()
    {
        _smelterRecipes = smelterRecipes;
    }
}
