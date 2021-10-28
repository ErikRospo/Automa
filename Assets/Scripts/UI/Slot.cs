using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;

[System.Serializable]
public class Slot
{
    // Inventory slot variables
    public Item item;
    public int amount;

    // Button UI variables
    public ButtonManagerBasicIcon button;
    public TextMeshProUGUI textAmount;
    public int slotNumber;
    public bool isHotbarSlot;
    public bool isSuitSlot;

    // Parameterized / default constructors
    public Slot(Item item, int amount) { SetSlot(item, amount); }
    public Slot() { SetSlot(null, 0); }

    // Start is called before the first frame update
    void Start()
    {
        Events.active.RegisterInventorySlot(this);
    }

    public void OnButtonPress()
    {
        UIEvents.active.InventorySlotClick(this);
    }

    public void SetSlot(Item item, int amount)
    {
        // Update UI elements
        if (item == null || this.amount <= 0)
        {
            this.item = null;
            this.amount = 0;
            button.buttonIcon = SpritesManager.GetSprite("Empty");
            textAmount.text = "";
        }
        else
        {
            this.item = item;
            this.amount = amount;
            button.buttonIcon = SpritesManager.GetSprite(item.name);
            textAmount.text = "x" + amount;
        }
    }
 }
