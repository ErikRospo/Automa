using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Inventory this UI is looking at
    public Inventory inventory;

    // Mouse variables
    public Transform mouse;
    public Image mouseIcon;
    public ItemData mouseItem;
    public int mouseAmount;

    // Sets the inventory
    public void SetInventory()
    {

    }

    // Sets the mouse item
    public void SetMouseItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
        {
            mouseItem = null;
            mouseAmount = 0;
            mouseIcon.sprite = SpritesManager.GetSprite("Empty");
        }
        else if (item != null)
        {
            mouseItem = item;
            mouseAmount = amount;
            mouseIcon.sprite = SpritesManager.GetSprite(mouseItem.name);
        }
    }

    // Press method
    public void OnSlotClicked(Slot slot)
    {
        
    }
}
