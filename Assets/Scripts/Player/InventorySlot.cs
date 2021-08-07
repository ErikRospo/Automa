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

        Events.current.RegisterInventorySlot(this);
    }

    public void SetItem(Inventory.InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        icon.sprite = Resources.Load<Sprite>("Icons/" + inventoryItem.item.name);
        amount.text = inventoryItem.amount.ToString();
    }

    public Inventory.InventoryItem TakeItem()
    {
        Inventory.InventoryItem holder = inventoryItem;
        RemoveItem();
        return holder;
    }

    public void RemoveItem()
    {
        inventoryItem = new Inventory.InventoryItem();
        icon.sprite = Resources.Load<Sprite>("Icons/Empty");
        amount.text = "";
    }
}
