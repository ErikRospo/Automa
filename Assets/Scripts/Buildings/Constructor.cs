using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor : Building
{
    public Recipe recipe;
    public Dictionary<EntityData, int> inputHolding;
    [HideInInspector] public int outputHolding;
    [HideInInspector] public CraftingHandler.ActiveCrafters crafter;
    [HideInInspector] public bool isCrafting = false;

    private void Start()
    {
        // Setup constructor
        SetRecipe(recipe);
        SetupPositions();
        CheckNearbyBuildings();

        GetComponent<SpriteRenderer>().receiveShadows = true;
    }

    // Set the recipe
    public void SetRecipe(Recipe recipe)
    {
        // Check if already crafting
        if (crafter != null)
        {
            CraftingHandler.RemoveCrafter(crafter);
            crafter = null;
            isCrafting = false;
        }

        this.recipe = recipe;
        inputHolding = new Dictionary<EntityData, int>();

        if (recipe.input.Length > 0)
            foreach (RecipeItem item in recipe.input)
                inputHolding.Add(item.item, 0);

        CheckStorage();

        // Update all input targets
        foreach(IOClass input in inputs) 
            if (input.building != null)
                input.building.UpdateBins();
    }

    // Adds an item to the internal crafter storage
    public void AddItem(EntityData itemToInput, int amountToAdd)
    {
        if (CheckItem(itemToInput))
        { 
            if (inputHolding.ContainsKey(itemToInput)) inputHolding[itemToInput] += amountToAdd;
            else inputHolding.Add(itemToInput, amountToAdd);
            if (!isCrafting) CheckStorage();
        }
    }

    // Crafts the chosen recipe
    public void CraftItem()
    {
        crafter = null;
        IOClass output = outputs[0];
        ItemData itemToOutput = recipe.output[0].item;

        if (output.bin == null)
        {
            output.bin = EntityHandler.active.RegisterEntity(itemToOutput, output.binPosition, Quaternion.identity);
            if (output.bin != null) UpdateBins();

            if (recipe.output[0].amount > 1)
                AddOutputItem(itemToOutput, recipe.output[0].amount - 1);
        }
        else AddOutputItem(itemToOutput, recipe.output[0].amount);

        for (int i = 0; i < recipe.input.Length; i++)
        {
            if (inputHolding.ContainsKey(recipe.input[i].item))
                inputHolding[recipe.input[i].item] -= recipe.input[i].amount;
        }

        isCrafting = false;
        CheckStorage();

        // Update all input targets
        foreach (IOClass input in inputs)
            if (input.building != null)
                input.building.UpdateBins();
    }

    public void AddOutputItem(ItemData item, int amount)
    {
        if (outputHolding + amount <= item.maxStackSize) outputHolding += amount;
    }

    // Check the internal storage. If enough materials, craft the item
    public void CheckStorage()
    {
        // Check if it's already crafting
        if (isCrafting) return;

        // Check the outputs of this building
        for(int i = 0; i < recipe.output.Length; i++)
            if (outputHolding + recipe.output[i].amount >= recipe.output[i].item.maxStackSize) return;

        // Check holding to see if there's enough to craft an item
        for (int i = 0; i < recipe.input.Length; i++)
            if (!inputHolding.ContainsKey(recipe.input[i].item) ||
                inputHolding[recipe.input[i].item] < recipe.input[i].amount) return;

        // If both checks pass, register crafting
        crafter = CraftingHandler.RegisterCrafting(this);
        isCrafting = true;
    }

    // Update entity bins
    public override void UpdateBins()
    {
        // Checks the front container
        if (outputs[0].bin != null && outputs[0].building != null && outputs[0].building.acceptingEntities)
        {
            if (outputs[0].building.InputEntity(outputs[0].bin))
            {
                outputs[0].bin = null;
                outputs[0].reserved = false;
                CheckStorage();
            }
        }

        // Checks output storage
        if (outputs[0].bin == null && outputHolding > 0)
        {
            outputHolding -= 1;
            outputs[0].bin = EntityHandler.active.RegisterEntity(recipe.output[0].item, outputs[0].binPosition, Quaternion.identity);
            if (outputs[0].bin != null) UpdateBins();
        }
    }
    
    // Check an item being inputted
    public bool CheckItem(EntityData item)
    {
        for (int i = 0; i < recipe.input.Length; i++)
            if (item == recipe.input[i].item) 
                return true;
        return false;
    }

    // Called when an entity is ready to be sent 
    public override bool InputEntity(ItemEntity entity)
    {
        if (CheckItem(entity.item))
        {
            if (inputHolding.ContainsKey(entity.item) &&
                inputHolding[entity.item] + 1 < entity.item.maxStackSize)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i].building == entity.lastBuilding /*|| inputs[i].binTargetPosition == entity.lastBuilding.transform.position*/)
                        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[i].binPosition, this);
                }
                return true;
            }
        }
        return false;
    }

    // Called when entity arrives at input bin
    public override void ReceiveEntity(ItemEntity entity)
    {
        // Add entity to internal storage and move it to output position
        AddItem(entity.item, 1);
        Recycler.AddRecyclable(entity.transform);
    }
}
