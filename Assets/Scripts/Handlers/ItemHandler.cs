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
    public struct Recipes
    {
        public string name;
        public RecipeItem[] input;
        public RecipeItem[] output;
        public Building madeIn;
        public int time;
        public bool unlocked;
    }

    public Recipes[] recipes;
    public static Recipes[] _recipes;

    private void Start()
    {
        _recipes = recipes;
    }
}
