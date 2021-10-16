using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RecipeButton : MonoBehaviour
{
    public ButtonManagerBasicWithIcon button;

    public void SetRecipe(Recipe recipe)
    {
        button.buttonText = recipe.name;
        button.buttonIcon = SpritesManager.GetSprite(recipe.output[0].item.name);
        button.UpdateUI();
    }
}
