using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    public class ActiveCrafters
    {
        public ActiveCrafters(Constructor crafter, float time)
        {
            this.crafter = crafter;
            this.time = time;
        }

        public Constructor crafter;
        public float time;
    }
    public static List<ActiveCrafters> crafters;

    void Start()
    {
        crafters = new List<ActiveCrafters>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < crafters.Count; i++)
        {
            if (CheckBuilding(i))
            {
                crafters[i].time -= Time.deltaTime;
                if (crafters[i].time <= 0)
                {
                    FinishCrafting(i);
                    i--;
                }
            }
            else i--;
        }
    }

    public static ActiveCrafters RegisterCrafting(Constructor crafter)
    {
        ActiveCrafters activeCrafter = new ActiveCrafters(crafter, crafter.recipe.time);
        crafters.Add(activeCrafter);
        return activeCrafter;
    }

    private void FinishCrafting(int i)
    {
        crafters[i].crafter.CraftItem();
        crafters.RemoveAt(i);
    }

    private bool CheckBuilding(int i)
    {
        if (crafters[i].crafter == null)
        {
            crafters.RemoveAt(i);
            return false;
        }
        return true;
    }
}
