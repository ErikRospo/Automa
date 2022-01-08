using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    // Constructor
    public Inventory(int size, bool giveStartingItems = false)
    {
        // Create inventory
        items = new List<Item>();
        for (int i = 0; i < size; i++)
            items.Add(new Item());

        // Give starting items
        if (giveStartingItems)
        {
            List<Item> startingItems = GameManager.active.FetchStartingItems();
            foreach (Item item in startingItems) AddItem(item);
        }
    }

    // Size of the inventory
    public int size;

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

    // Get items from inventory
    public List<Item> GetItems() { return items; }
}
