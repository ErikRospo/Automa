using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;

[System.Serializable]
public class Slot : MonoBehaviour
{
    // Inventory slot variables
    [HideInInspector] public Item item;
    [HideInInspector] public int amount;

    // Button UI variables
    public ButtonManagerBasicIcon button;
    public TextMeshProUGUI textAmount;
    public int slotNumber;
    public bool isHotbarSlot;
    public bool isSuitSlot;

    // Start is called before the first frame update
    void Start()
    {
        Events.active.RegisterInventorySlot(this);
    }

    public void OnButtonPress()
    {
        UIEvents.active.InventorySlotClick(this);
    }

    public int SetSlot(Item item, int amount)
    {
        // Overflow value
        int overflow = 0;

        // Update UI elements
        if (item == null || amount <= 0)
        {
            // Reset slot
            this.item = null;
            this.amount = 0;
            button.buttonIcon = SpritesManager.GetSprite("Empty");
            textAmount.text = "";
            button.UpdateUI();
        }
        else
        {
            // Set item
            this.item = item;

            // Calculate overflow (if any)
            if (this.amount + amount > item.maxStackSize)
            {
                overflow = (this.amount + amount) - item.maxStackSize;
                this.amount = item.maxStackSize;
            }
            else this.amount += amount;
            
            // Update UI elements
            button.buttonIcon = SpritesManager.GetSprite(item.name);
            textAmount.text = "x" + this.amount;
            button.UpdateUI();
        }

        return overflow;
    }
 }
