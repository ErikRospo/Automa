using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static UIEvents active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    public event Action<InterfaceType, Player> onOpenInterface;
    public void OpenInterface(InterfaceType type, Player player)
    {
        if (onOpenInterface != null)
            onOpenInterface(type, player);
    }

    public event Action<Slot> onInventorySlotClick;
    public void InventorySlotClick(Slot slot)
    {
        if (onInventorySlotClick != null)
            onInventorySlotClick(slot);
    }

    // Invoked when a bullet is fired
    public event Action<Recipe> onAddRecipe;
    public void AddRecipe(Recipe recipe)
    {
        if (onAddRecipe != null)
            onAddRecipe(recipe);
    }

    // Invoked when a bullet is fired
    public event Action onBakeRecipes;
    public void BakeRecipes()
    {
        if (onBakeRecipes != null)
            onBakeRecipes();
    }

    // Invoked when a building is clicked
    public event Action<Constructor> onConstructorClicked;
    public void ConstructorClicked(Constructor constructor)
    {
        if (onConstructorClicked != null)
            onConstructorClicked(constructor); 
    }
}
