using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    public class ActiveCrafters
    {
        public ActiveCrafters(Crafter crafter, int time)
        {
            this.crafter = crafter;
            this.time = time;
        }

        public Crafter crafter;
        public int time;
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
                crafters[i].time -= 1;
                if (crafters[i].time < 1)
                {
                    FinishCrafting(i);
                    i--;
                }
            }
            else i--;
        }
    }

    public static void RegisterCrafting(Crafter crafter)
    {
        crafters.Add(new ActiveCrafters(crafter, crafter.recipe.time));
    }

    private void FinishCrafting(int i)
    {

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
