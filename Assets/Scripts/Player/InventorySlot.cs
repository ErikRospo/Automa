using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Inventory.InventoryItem inventoryItem;
    [HideInInspector] public Image icon;
    [HideInInspector] public TextMeshProUGUI amount;
    public int slotNumber;

    private void Start()
    {
        try
        {
            icon = transform.GetChild(0).GetComponent<Image>();
            amount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        catch
        {
            Debug.LogError("One or more inventory slots is missing either an image or TMP object!");
            return;
        }

        Events.active.RegisterInventorySlot(this);
        Events.active.onRequestInventorySlots += SendInventorySlot;
    }

    // Sends an inventory slot back to the requesting script
    private void SendInventorySlot(Inventory inventory)
    {
        inventory.inventorySlots.Add(this);
    }

    // Sets an inventory slot to a specified item
    public void SetItem(Inventory.InventoryItem inventoryItem)
    {
        Debug.Log(inventoryItem.item.name);
        this.inventoryItem = inventoryItem;
        icon.sprite = Resources.Load<Sprite>("Sprites/Items/" + inventoryItem.item.name);
        amount.text = inventoryItem.amount.ToString();
    }

    // Takes an item from the specified inventory slot
    public Inventory.InventoryItem TakeItem()
    {
        Inventory.InventoryItem holder = inventoryItem;
        RemoveItem();
        return holder;
    }

    // Removes an item from the specified inventory slot
    public void RemoveItem()
    {
        inventoryItem = new Inventory.InventoryItem();
        icon.sprite = Resources.Load<Sprite>("Sprites/Interface/Empty");
        amount.text = "";
    }
}
