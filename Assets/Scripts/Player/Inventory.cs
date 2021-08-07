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
    [SerializeField] private int inventorySize;

    // If inventory UI is available
    public List<InventorySlot> inventorySlots;

    // Initial setup
    private void Start()
    {
        inventorySize = 9;
        Events.current.onRegisterInventorySlot += OnRegisterHotbarSlot;
    }

    // Inventory slot listener
    private void OnRegisterHotbarSlot(InventorySlot slot)
    {
        inventorySlots.Add(slot);
    }

    // Updates server on item being added
    [Command]
    public void CmdAddItem(Item item, int amount)
    {
        // Check for an available spot
        for (int i=0; i < inventorySize; i++)
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
                    amount = 0;
                }
            }
            else if (inventory.TryGetValue(i, out InventoryItem holder))
            {
                int spotsAvailable = item.maxStackSize - holder.amount;
                if (amount > spotsAvailable)
                {
                    AddItem(i, item, spotsAvailable);
                    amount -= spotsAvailable;
                }
                else
                {
                    AddItem(i, item, amount);
                    amount = 0;
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
        {
            inventory = player.inventory;

        }
    }

    // Adds an item to a players inventory
    private void AddItem(int slot, Item item, int amount)
    {
        InventoryItem inventoryItem = new InventoryItem(item, amount);
        inventory.Add(slot, inventoryItem);

        if (inventorySlots.Count > slot)
            inventorySlots[slot].SetItem(inventoryItem);
        else Debug.LogError("Not enough slots available to add item!")
    }
}
