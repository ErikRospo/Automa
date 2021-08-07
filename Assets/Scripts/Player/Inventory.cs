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

    private void OnRegisterHotbarSlot(InventorySlot slot)
    {
        inventorySlots.Add(slot);
    }

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
                    inventory.Add(i, new InventoryItem(item, item.maxStackSize));
                    UpdateUI(Resources.Load<Sprite>("Sprites/World/" + item.name), i, item.maxStackSize);
                    amount -= item.maxStackSize;
                }
                else
                {
                    inventory.Add(i, new InventoryItem(item, amount));
                    UpdateUI(Resources.Load<Sprite>("Sprites/World/" + item.name), i, amount);
                    amount = 0;
                }
            }
            else if (inventory.TryGetValue(i, out InventoryItem holder))
            {
                int spotsAvailable = item.maxStackSize - holder.amount;
                if (amount > spotsAvailable)
                {
                    inventory.Add(i, new InventoryItem(item, spotsAvailable));
                    UpdateUI(Resources.Load<Sprite>("Sprites/World/" + item.name), i, item.maxStackSize);
                    amount -= spotsAvailable;
                }
                else
                {
                    inventory.Add(i, new InventoryItem(item, amount));
                    UpdateUI(Resources.Load<Sprite>("Sprites/World/" + item.name), i, amount);
                    amount = 0;
                }
            }
        }

        RpcUpdateInventory(this);
    }

    [ClientRpc]
    public void RpcUpdateInventory(Inventory player)
    {
        if (player == this)
        {
            inventory = player.inventory;

        }
    }

    private void UpdateUI(Sprite icon, int slot, int amount)
    {

    }
}
