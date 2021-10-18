using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipIO : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI amount;

    public void SetTooltipIO(Sprite icon, string amount)
    {
        image.sprite = icon;
        this.amount.text = amount;
    }
}
