using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RecipeButton : MonoBehaviour
{
    public ButtonManagerBasicWithIcon button;
    [HideInInspector] private Recipe recipe;

    public void SetRecipe()
    {
        Events.active.SetRecipe(recipe);
    }

    public void SetInfo(Recipe recipe)
    {
        this.recipe = recipe;
        button.buttonText = recipe.name;
        button.buttonIcon = SpritesManager.GetSprite(recipe.output[0].item.name);
        button.UpdateUI();
    }
}
