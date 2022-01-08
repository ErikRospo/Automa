using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    // Parameterized constructor
    public Item(ItemData type = null, int amount = 0)
    {
        this.type = type;
        this.amount = amount;
    }

    // Inventory slot variables
    public ItemData type;
    public int amount;

    // Add to this item
    public int Add(int amountToAdd, bool allowOverflow = false)
    {
        // Create overflow tracker
        int overflow = 0;

        // Calculate overflow (if any)
        int itemStackSize = GetStackSize();
        if (!allowOverflow && amount + amountToAdd > itemStackSize)
        {
            overflow = (amount + amountToAdd) - itemStackSize;
            amount = itemStackSize;
        }
        else amount += amountToAdd;

        // Return the overflow
        return overflow;
    }

    // Remove an amount from this item
    public void Remove(int amountToRemove)
    {
        amount -= amountToRemove;
        if (amount < 0) amount = 0;
    }

    // Get max stack size
    public int GetStackSize() { return type.maxStackSize; }

    // Get item sprite
    public Sprite GetIcon() { return type.icon; }
}
