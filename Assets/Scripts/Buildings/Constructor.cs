using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor : Building
{
    public Machine machine;
    public Recipe recipe;
    public Dictionary<Item, int> inputHolding;
    [HideInInspector] public int outputHolding;
    [HideInInspector] public CraftingHandler.ActiveCrafters crafter;
    [HideInInspector] public bool isCrafting = false;

    private void Start()
    {
        SetupRotation();
        SetupPositions();
        CheckNearbyBuildings();
        SetRecipe(recipe);
    }

    // Set the recipe
    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        inputHolding = new Dictionary<Item, int>();

        foreach(RecipeItem item in recipe.input)
            inputHolding.Add(item.item, 0);
    }

    // Adds an item to the internal crafter storage
    public void AddItem(Item itemToInput, int amountToAdd)
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
        Item itemToOutput = recipe.output[0].item;

        if (output.bin == null)
        {
            output.bin = EntityHandler.active.RegisterEntity(itemToOutput, output.position, Quaternion.identity);
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
    }

    public void AddOutputItem(Item item, int amount)
    {
        if (outputHolding + amount <= item.maxStackSize) outputHolding += amount;
    }

    // Check the internal storage. If enough materials, craft the item
    public void CheckStorage()
    {
        // Check the outputs of this building
        for(int i = 0; i < recipe.output.Length; i++)
            if (outputHolding + recipe.output[i].amount >= recipe.output[i].item.maxStackSize) return;

        // Check holding to see if there's enough to craft an item
        for (int i = 0; i < recipe.input.Length; i++)
            if (!inputHolding.TryGetValue(recipe.input[i].item, out int amount) 
                || amount < recipe.input[i].amount) return;

        crafter = CraftingHandler.RegisterCrafting(this);
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

        // Checks output storage
        if (outputs[0].bin == null && outputHolding > 0)
        {
            outputHolding -= 1;
            outputs[0].bin = EntityHandler.active.RegisterEntity(recipe.output[0].item, outputs[0].position, Quaternion.identity);
            if (outputs[0].bin != null) UpdateBins();
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
        for (int i = 0; i < inputs.Length; i++)
            if (target.transform.position == inputs[i].tilePosition)
                inputs[i].target = target;
    }

    public override void SetOutputTarget(Building target)
    {
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
            if (!inputHolding.ContainsKey(entity.item) || (inputHolding.TryGetValue(entity.item, out int amount) && amount < entity.item.maxStackSize))
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i].target == entity.lastBuilding)
                        entity.MoveTo(ResearchHandler.conveyorSpeed, inputs[i].position, this);
                }
                return true;
            }
        }
        return false;
    }

    // Called when entity arrives at input bin
    public override void ReceiveEntity(Entity entity)
    {
        // Add entity to internal storage and move it to output position
        AddItem(entity.item, 1);
        Recycler.AddRecyclable(entity.transform);
    }
}
