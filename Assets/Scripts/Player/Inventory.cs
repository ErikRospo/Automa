using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    // Inventory variables 
    public struct InventoryItem
    { 
        public Item item;
        public int amount;

        // Constructor
        public InventoryItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
    public Dictionary<int, InventoryItem> inventory = new Dictionary<int, InventoryItem>();

    // If inventory UI is available
    public List<InventorySlot> inventorySlots;

    // Initial setup
    private void Start()
    {
        // Will grab all the currently available inventory slots
        Events.current.onRegisterInventorySlot += OnRegisterInventorySlot;
        if (inventorySlots.Count == 0)
            Events.current.RequestInventorySlots(this);
    }

    // Inventory slot listener
    private void OnRegisterInventorySlot(InventorySlot slot)
    {
        inventorySlots.Add(slot);
    }

    // Updates server on item being added
    [Command]
    public void CmdAddItem(Item item, int amount)
    {
        // Check for an available spot
        for (int i=0; i < inventorySlots.Count; i++)
        {
            if (!inventory.ContainsKey(i))
            {
                if (amount > item.maxStackSize)
                {
                    AddItem(i, item, item.maxStackSize);
                    amount -= item.maxStackSize;
                }
                else
                {
                    AddItem(i, item, amount);
                    break;
                }
            }
            else if (inventory.TryGetValue(i, out InventoryItem holder))
            {
                int spotsAvailable = item.maxStackSize - holder.amount;
                if (amount > spotsAvailable)
                {
                    AddItem(i, item, item.maxStackSize);
                    amount -= spotsAvailable;
                }
                else
                {
                    AddItem(i, item, amount + holder.amount);
                    break;
                }
            }
        }

        RpcUpdateInventory(this);
    }

    // Updates all other players on inventory change
    [ClientRpc]
    public void RpcUpdateInventory(Inventory player)
    {
        if (player == this)
            inventory = player.inventory;
    }

    // Adds an item to a players inventory
    private void AddItem(int slot, Item item, int amount)
    {
        // Adds the item to the backend inventory database
        InventoryItem inventoryItem = new InventoryItem(item, amount);
        if (!inventory.ContainsKey(slot)) inventory.Add(slot, inventoryItem);
        else inventory[slot] = inventoryItem;

        // Attempts to place the item in the inventory UI
        foreach (InventorySlot inventorySlot in inventorySlots)
            if (inventorySlot.slotNumber == slot)
            {
                inventorySlot.SetItem(inventoryItem);
                return;
            }

        // If slot not found, debug to console
        Debug.LogError("Slot #" + slot + " could not be found. Item only added to backend inventory.");
    }
}
