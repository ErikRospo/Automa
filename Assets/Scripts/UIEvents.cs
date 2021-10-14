using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    // Invoked when a bullet is fired
    public event Action<Recipe> addRecipe;
    public void AddRecipe(Recipe recipe)
    {
        if (addRecipe != null)
            AddRecipe(recipe);
    }

    // Invoked when a bullet is fired
    public event Action bakeRecipes;
    public void BakeRecipes()
    {
        if (bakeRecipes != null)
            BakeRecipes();
    }
}
