using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Inventory.InventoryItem inventoryItem;
    public Image icon;

    private void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        Events.current.RegisterHotbarSlot(this);
    }

    public void SetItem(Inventory.InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        icon.sprite = Resources.Load<Sprite>("Icons/" + inventoryItem.item.name);
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
    }
}
