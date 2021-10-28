using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : NetworkBehaviour
{
    // Active instance
    public static Inventory active;

    // Mouse variables
    public Transform mouse;
    public Image mouseIcon;
    public Item mouseItem;
    public int mouseAmount;

    // If inventory UI is available
    private List<Slot> inventorySlots;
    private List<Slot> hotbarSlots;
    private List<Slot> suitSlots;

    // Initial setup
    private void Start()
    {
        // Set active instance
        active = this;

        // Create new lists
        inventorySlots = new List<Slot>();
        hotbarSlots = new List<Slot>();
        suitSlots = new List<Slot>();

        // Will grab all currently available inventory slots
        UIEvents.active.onInventorySlotClick += OnSlotClicked;
        Events.active.onRegisterInventorySlot += OnRegisterInventorySlot;
        if (inventorySlots.Count == 0) Events.active.RequestInventorySlots(this);
    }

    // Press method
    public void OnSlotClicked(Slot slot)
    {
        // If item selected, set slot
        if (slot.item == null && mouseItem != null)
        {
            slot.SetSlot(mouseItem, mouseAmount);
            SetMouseItem(null, 0);
        }
        else if (slot.item != null)
        {
            SetMouseItem(slot.item, slot.amount);
            slot.SetSlot(null, 0);
        }
    }

    // Sets the mouse item
    public void SetMouseItem(Item item, int amount)
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

    // Inventory slot listener
    private void OnRegisterInventorySlot(Slot slot)
    {
        if (slot.isHotbarSlot)
            hotbarSlots.Add(slot);
        else if (slot.isSuitSlot)
            suitSlots.Add(slot);
        else
            inventorySlots.Add(slot);
    }

    // Updates server on item being added
    //[Command]
    public void CmdAddItem(Item item, int amount)
    {
        // Check for an available spot
        int holdingAmount = amount;
        foreach(Slot slot in inventorySlots) 
        { 
            if (slot.item == null || (slot.item == item && slot.amount < slot.item.maxStackSize))
                holdingAmount = slot.SetSlot(item, amount);
            if (holdingAmount <= 0) break;
        }
    }
}
