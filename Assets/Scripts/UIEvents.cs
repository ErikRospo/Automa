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

    // Invoked when a building is clicked
    public event Action<Constructor> onBuildingClicked;
    public void BuildingClicked(Constructor constructor)
    {
        if (onBuildingClicked != null)
            onBuildingClicked(constructor); 
    }
}
