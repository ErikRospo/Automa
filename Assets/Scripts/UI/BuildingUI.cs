using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public Manager manager;

    public TMP_Dropdown recipeSelector;
    [HideInInspector] public List<string> recipesToAdd;
    [HideInInspector] public List<Recipe> recipes;

    public static BuildingUI active;

    // Start is called before the first frame update
    public void Start()
    {
        if (this != null) active = this;

        UIEvents.active.onAddRecipe += AddRecipe;
        UIEvents.active.onBakeRecipes += BakeRecipes;
        UIEvents.active.onConstructorClicked += DisplayConstructorInfo;

        recipesToAdd = new List<string>();
        recipes = new List<Recipe>();

        manager.GenerateRecipes();
    }

    public void DisplayConstructorInfo(Constructor constructor)
    {

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
