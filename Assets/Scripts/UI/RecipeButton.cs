using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class RecipeButton : MonoBehaviour
{
    public ButtonManagerBasicWithIcon button;
    [HideInInspector] private Recipe recipe;
    public TooltipContent tooltip;

    public void SetRecipe()
    {
        Events.active.SetRecipe(recipe);
    }

    public void SetInfo(Recipe recipe, GameObject tooltipRect, TextMeshProUGUI tooltipDesc)
    {
        this.recipe = recipe;
        button.buttonText = recipe.name;
        button.buttonIcon = SpritesManager.GetSprite(recipe.output[0].item.name);
        button.UpdateUI();

        tooltip.tooltipRect = tooltipRect;
        tooltip.descriptionText = tooltipDesc;
    }

    public void SetTooltip()
    {
        tooltip.description = recipe.output[0].item.description;
    }
}
