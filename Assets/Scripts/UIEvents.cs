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
    public void AddRecipe()
    {
        if (addRecipe != null)
            AddRecipe();
    }
}
