using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string name;
    public RecipeItem[] input;
    public RecipeItem[] output;
    public Building madeIn;
    public int time;
    public bool unlocked;
}
