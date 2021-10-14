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
    public event Action<Recipe> onAddRecipe;
    public void AddRecipe(Recipe recipe)
    {
        if (onAddRecipe != null)
            onAddRecipe(recipe);
    }

    // Invoked when a bullet is fired
    public event Action onBakeRecipes;
    public void BakeRecipes()
    {
        if (onBakeRecipes != null)
            onBakeRecipes();
    }
}
