using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor : Building
{
    public Crafter crafter;
    public Recipe recipe;
    public Dictionary<Item, int> holding;
    [HideInInspector] public bool isCrafting = false;

    private void Start()
    {
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();
        holding = new Dictionary<Item, int>();
    }

    // Adds an item to the internal crafter storage
    public void AddItem(Item itemToInput, int amountToAdd)
    {
        if (CheckItem(itemToInput))
        { 
            if (holding.ContainsKey(itemToInput)) holding[itemToInput] += amountToAdd;
            else holding.Add(itemToInput, amountToAdd);
            if (!isCrafting) CheckStorage();
        }
    }

    // Crafts the chosen recipe
    public void CraftItem()
    {
        for (int i = 0; i < recipe.output.Length; i++)
        {
            IOClass output = outputs[recipe.output[i].index];
            output.bin = EntityHandler.active.RegisterEntity(recipe.output[i].item, 
                output.position, Quaternion.identity);
            if (output.bin != null) UpdateBins();
        }

        isCrafting = false;
        CheckStorage();
    }

    // Check the internal storage. If enough materials, craft the item
    public void CheckStorage()
    {
        // Check the outputs of this building
        for(int i = 0; i < recipe.output.Length; i++)
            if (outputs[recipe.output[i].index].binAmount + recipe.output[i].amount 
                >= recipe.output[i].item.maxStackSize) return;

        // Check holding to see if there's enough to craft an item
        for (int i = 0; i < recipe.input.Length; i++)
            if (!holding.TryGetValue(recipe.input[i].item, out int amount) 
                || amount < recipe.input[i].amount) return;

        CraftingHandler.RegisterCrafting(this);
        isCrafting = true;
    }

    // Update entity bins
    public override void UpdateBins()
    {
        // Checks the front container
        if (outputs[0].bin != null && outputs[0].target != null && outputs[0].target.acceptingEntities)
        {
            if (outputs[0].target.InputEntity(outputs[0].bin))
            {
                outputs[0].bin = null;
                outputs[0].reserved = false;
            }
        }
    }
    
    // Check an item being inputted
    public bool CheckItem(Item item)
    {
        for (int i = 0; i < recipe.input.Length; i++)
            if (item == recipe.input[i].item) 
                return true;
        return false;
    }

    public override void SetInputTarget(Building target)
    {
        Debug.Log("Reee?");

        for (int i = 0; i < inputs.Length; i++)
            if (target.transform.position == inputs[i].tilePosition)
                inputs[i].target = target;
    }

    public override void SetOutputTarget(Building target)
    {
        Debug.Log("Reee213123?");

        for (int i = 0; i < outputs.Length; i++)
            if (target.transform.position == outputs[i].tilePosition)
                outputs[i].target = target;

        UpdateBins();
    }

    // Called when an entity is ready to be sent 
    public override bool InputEntity(Entity entity)
    {
        if (CheckItem(entity.item))
        {
            if (!holding.ContainsKey(entity.item) || (holding.TryGetValue(entity.item, out int amount) && amount < entity.item.maxStackSize))
            {
                entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[0].position, this);
                return true;
            }
        }
        return false;
    }

    public override void ReceiveEntity(Entity entity)
    {
        // Add entity to internal storage and move it to output position
        AddItem(entity.item, 1);
        Recycler.AddRecyclable(entity.transform);
    }
}
