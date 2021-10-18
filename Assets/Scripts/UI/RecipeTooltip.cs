using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;

public class RecipeTooltip : MonoBehaviour
{
    // Tablet tooltip system
    public TextMeshProUGUI tooltipTime;
    public TextMeshProUGUI tooltipDesc;
    public List<TooltipIO> tooltipInputs;
    public List<TooltipIO> tooltipOutputs;

    // Set tooltip content
    public void SetTooltipContent(Recipe recipe)
    {
        // Set text
        tooltipTime.text = ((int)recipe.time).ToString() + "s";
        tooltipDesc.text = recipe.output[0].item.description;

        // Disable input and outputs
        foreach (TooltipIO input in tooltipInputs)
            input.gameObject.SetActive(false);
        foreach (TooltipIO output in tooltipOutputs)
            output.gameObject.SetActive(false);

        // Re-enalbe inputs and outputs that are needed
        for (int i = 0; i < recipe.input.Length; i++)
        {
            tooltipInputs[i].gameObject.SetActive(true);
            tooltipInputs[i].SetTooltipIO(SpritesManager.GetSprite(recipe.input[i].item.name), recipe.input[i].amount.ToString());
        }
        for (int i = 0; i < recipe.output.Length; i++)
        {
            tooltipOutputs[i].gameObject.SetActive(true);
            tooltipOutputs[i].SetTooltipIO(SpritesManager.GetSprite(recipe.output[i].item.name), recipe.output[i].amount.ToString());
        }
    }
}
