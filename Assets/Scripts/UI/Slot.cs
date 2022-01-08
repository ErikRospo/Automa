using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;

[System.Serializable]
public class Slot : MonoBehaviour
{
    // Button UI variables
    public Item item;
    public ButtonManagerBasicIcon button;
    public TextMeshProUGUI textAmount;

    /// <summary>
    /// Sets the specified slot to a new item
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public void Set(Item newItem)
    {
        // Update UI elements
        if (newItem != null)
        {
            item = newItem;
            UpdateUI();
        }
        else Clear();
    }

    /// <summary>
    /// Adds a specified amount to this item slot.
    /// </summary>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public int Add(Item newItem, bool allowOverflow = false)
    {
        // Get overflow amount
        int overflow = 0;

        // Update UI elements
        if (newItem != null)
        {
            // Set item if type is not the same
            if (item.type != newItem.type)
                item = new Item(newItem.type);

            // Add item amount
            overflow = item.Add(newItem.amount, allowOverflow);

            // Update UI elements
            UpdateUI();
        }

        // Return any overflow
        return overflow;
    }

    /// <summary>
    /// Resets this item slot.
    /// </summary>
    public void Clear()
    {
        // Reset slot
        item = new Item();
        UpdateUI(true);
    }

    /// <summary>
    /// Removes a specified amount from this item slot.
    /// </summary>
    /// <param name="amount"></param>
    public void Remove(int amount) { item.Remove(amount); }

    /// <summary>
    /// Checks if the item slot is the same type as another item
    /// </summary>
    /// <param name="item"></param>
    public bool Check(ItemData itemData) { return item.type == itemData; }

    /// <summary>
    /// Updates the UI of the item slot.
    /// </summary>
    public void UpdateUI(bool clear = false)
    {
        if (clear)
        {
            // Clear the UI elements
            button.buttonIcon = SpritesManager.GetSprite("Empty");
            textAmount.text = "";
            button.UpdateUI();
        }
        else
        {
            // Update UI elements
            button.buttonIcon = item.GetIcon();
            textAmount.text = "x" + item.amount;
            button.UpdateUI();
        }
    }
 }
