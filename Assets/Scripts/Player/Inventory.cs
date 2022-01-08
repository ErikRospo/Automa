using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    // Constructor
    public Inventory(bool giveStartingItems)
    {
        if (giveStartingItems)
            items = new List<Item>(GameManager.active.FetchStartingItems());
        else items = new List<Item>();
    }

    // If inventory UI is available
    private List<Item> items = new List<Item>();

    // Add an item to an inventory
    public void AddItem(Item newItem)
    {
        // Check for an available spot
        int amountToAdd = newItem.amount;

        // Iterate through slots until added
        foreach(Item item in items) 
        {
            if (item.type == null)
            {
                item.type = newItem.type;
                amountToAdd = item.Add(amountToAdd);
            }
            else amountToAdd = item.Add(amountToAdd);

            if (amountToAdd <= 0) break;
        }
    }
}
