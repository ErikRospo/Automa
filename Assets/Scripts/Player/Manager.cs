using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public string path = "Scriptables/Recipes";

    // Generates recipes on run
    public void GenerateRecipes()
    {
        List<Recipe> recipes = Resources.LoadAll(path, typeof(Recipe)).Cast<Recipe>().ToList();
        Debug.Log("Loaded " + recipes.Count + " recipes from " + path);

        // Generate buildables
        for (int i = 0; i < recipes.Count; i++)
            UIEvents.active.AddRecipe(recipes[i]);
        UIEvents.active.BakeRecipes();
    }
}
