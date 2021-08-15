using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Building
{
    Item input;
    Item output;

    public void SetRecipe()
    {

    }

    public override bool PassEntity(Entity entity)
    {



        return true;
    }
}
