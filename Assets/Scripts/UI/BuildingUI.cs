using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public TMP_Dropdown recipeSelector;
    private List<string> recipesToAdd;
    private List<Recipe> recipes;

    // Start is called before the first frame update
    public void Start()
    {
        UIEvents.active.addRecipe += AddRecipe;
        UIEvents.active.bakeRecipes += BakeRecipes;

        recipesToAdd = new List<string>();
        recipes = new List<Recipe>();
    }

    // Adds a recipe on startup
    public void AddRecipe(Recipe recipe)
    {
        Debug.Log("Loaded recipe " + recipe.name);

        recipesToAdd.Add(recipe.name);
        recipes.Add(recipe);
    }

    // Bakes recipes into dropdown
    public void BakeRecipes()
    {
        recipeSelector.ClearOptions();
        recipeSelector.AddOptions(recipesToAdd);
    }
}
