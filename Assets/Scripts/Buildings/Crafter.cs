using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : Building
{
    public ItemHandler.Recipes recipe;
    public Dictionary<Item, int> holding;

    private void Start()
    {
        SetupPositions();
        nextTarget = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
    }

    public void AddItem(Item itemToInput, int amountToAdd)
    {
        Item input = recipe.input[0].item;
        if (itemToInput == input)
        {
            int amount = amountToAdd;
            if (holding.TryGetValue(itemToInput, out amount)) amount += amountToAdd;
            else holding.Add(itemToInput, amount);

            Debug.Log(amount + " | " + recipe.input[0].amount);

            // If enough entities in storage, begin smelting
            if (amount == recipe.input[0].amount && frontBin == null)
                CraftingHandler.RegisterCrafting(this);
        }
    }

    public void CraftItem()
    {
        holding = new Dictionary<Item, int>();

        if (nextTarget != null)
        {
            frontBin = EntityHandler.RegisterEntity(recipe.input[0].item.obj.transform, transform.position, Quaternion.identity);
            if (frontBin != null)
                if (nextTarget == null || !nextTarget.acceptingEntities || !nextTarget.PassEntity(frontBin))
                    outputReserved = true;
        }
    }

    public override void UpdateBins()
    {
        // Checks the front container
        if (frontBin != null && nextTarget != null && nextTarget.acceptingEntities)
        {
            if (nextTarget.PassEntity(frontBin))
            {
                frontBin = null;
                outputReserved = false;
            }
        }
    }

    public override bool PassEntity(Entity entity)
    {
        Item input = recipe.input[0].item;
        if (entity.item == input)
        {
            if (!holding.TryGetValue(entity.item, out int amount) || amount < input.maxStackSize)
            {
                EntityHandler.RegisterMovingEntity(ResearchHandler.conveyorSpeed, inputPositions[0], entity, this);
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
