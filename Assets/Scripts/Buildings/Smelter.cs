using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Building
{
    [HideInInspector] public ItemHandler.SmelterRecipe recipe;
    [HideInInspector] public Building nextTarget;
    [HideInInspector] public int holding;
    private List<Entity> entitiesInside;

    public override bool PassEntity(Entity entity)
    {
        Item input = recipe.input.item;
        if (entity.item == input && holding < input.maxStackSize)
        {
            EntityHandler.RegisterMovingEntity(ResearchHandler.conveyorSpeed, inputPositions[0].position, entity, this);
            return true;
        }
        return false;
    }

    public override void ReceiveEntity(Entity entity)
    {
        // Add entity to internal storage and move it to output position
        holding += 1;
        entity.transform.position = outputPositions[0].position;
        entitiesInside.Add(entity);

        // If enough entities in storage, begin smelting
        if (holding == recipe.input.amount)
            SmelterHandler.RegisterSmelter(this);
    }
}
