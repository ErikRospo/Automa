using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Inventory this UI is looking at
    public Inventory inventory;

    // List of slots being displayed
    public Transform slotList;
    public Slot slotObject;
    public List<Slot> slots;

    // Bounds of this inventory UI
    public int rows = 5;
    public float spacing = 70f;

    // Mouse variables
    public Transform mouse;
    public Image mouseIcon;
    public Item mouseItem;

    // Sets the inventory
    public void SetInventory(Inventory inventory)
    {
        // Set the new inventory
        this.inventory = inventory;
        ClearSlots();
    }

    // Set the slots
    public void SetSolts(List<Item> items)
    {
        // Check if new slots are needed
        if (items.Count > slots.Count)
            CreateSlots(items.Count - slots.Count);

        // Loop through slots already created
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Set(items[i]);
            slots[i].gameObject.SetActive(true);
        }
    }

    // Create slots
    public void CreateSlots(int amount)
    {
        // Create the new slots
        for (int i = slots.Count; i < slots.Count + amount; i++)
        {
            // Get column and row
            int column = i / rows;
            int row = (column * rows) + i;

            // Create the new slot
            Slot newSlot = Instantiate(slotObject, Vector2.zero, Quaternion.identity).GetComponent<Slot>();

            // Set the position relative to list
            newSlot.transform.SetParent(slotList);
            newSlot.transform.localScale = new Vector3(1, 1, 1);
            newSlot.transform.position = new Vector2(column * spacing, row * spacing);
        }
    }

    // Clear active slots
    public void ClearSlots()
    {
        foreach(Slot slot in slots)
        {
            slot.Clear();
            slot.gameObject.SetActive(false);
        }
    }

    // Sets the mouse item
    public void SetMouseItem(Item newItem)
    {
        if (newItem == null)
        {
            mouseItem = new Item();
            mouseIcon.sprite = SpritesManager.GetSprite("Empty");
        }
        else
        {
            mouseItem = newItem;
            mouseIcon.sprite = newItem.GetIcon();
        }
    }

    // Press method
    public void OnSlotClicked(Slot slot)
    {
        
    }
}
