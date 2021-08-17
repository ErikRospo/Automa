using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : Building
{
    public ItemHandler.Recipes recipe;
    public Dictionary<Item, int> holding;
    public bool isCrafting = false;

    private void Start()
    {
        outputReserved = false;
        SetupRotation();
        SetupPositions();
        holding = new Dictionary<Item, int>();
        nextTarget = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
    }

    public void AddItem(Item itemToInput, int amountToAdd)
    {
        Item input = recipe.input[0].item;
        if (itemToInput == input)
        {
            if (holding.ContainsKey(itemToInput)) holding[itemToInput] += amountToAdd;
            else holding.Add(itemToInput, amountToAdd);
            CheckStorage();
        }
    }

    public void CraftItem()
    {
        isCrafting = false;
        holding[recipe.input[0].item] -= recipe.input[0].amount;

        if (nextTarget != null)
        {
            frontBin = EntityHandler.RegisterEntity(recipe.output[0].item, outputPositions[0], Quaternion.identity);
            if (frontBin != null) UpdateBins();
        }

        CheckStorage();
    }

    public void CheckStorage()
    {
        if (holding.TryGetValue(recipe.input[0].item, out int amount))
        {
            if (amount >= recipe.input[0].amount && frontBin == null && !isCrafting)
            {
                CraftingHandler.RegisterCrafting(this);
                isCrafting = true;
            }
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
                //outputReserved = false;
            }
            //else outputReserved = true;
        }
    }

    public void CheckNearbyBuildings()
    {
        // Check the output tile
        Building building = BuildingHandler.TryGetBuilding(outputTilePositions[0]);
        if (building != null)
            nextTarget = building;

        // Check the input tile
        Conveyor conveyor = BuildingHandler.TryGetConveyor(inputTilePositions[0]);
        if (conveyor != null)
        {
            if (conveyor.transform.rotation == transform.rotation)
            {
                conveyor.nextTarget = this;
                conveyor.UpdateBins();
                previousTarget = conveyor;
            }
            else if (conveyor.isCorner) conveyor.CornerCheck(this);
        }
        else
        {
            building = BuildingHandler.TryGetBuilding(inputTilePositions[0]);
            if (building != null)
            {
                building.nextTarget = this;
                building.UpdateBins();
            }
        }
    }

    public override bool PassEntity(Entity entity)
    {
        Item input = recipe.input[0].item;
        if (entity.item == input)
        {
            if (!holding.ContainsKey(entity.item) || (holding.TryGetValue(entity.item, out int amount) && amount < input.maxStackSize))
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
