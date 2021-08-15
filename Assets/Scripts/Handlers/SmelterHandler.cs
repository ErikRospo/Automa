using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterHandler : MonoBehaviour
{
    public class ActiveSmelters
    {
        public ActiveSmelters(Smelter smelter, int time)
        {
            this.smelter = smelter;
            this.time = time;
        }

        public Smelter smelter;
        public int time;
    }
    public static List<ActiveSmelters> smelters;

    void Start()
    {
        smelters = new List<ActiveSmelters>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < smelters.Count; i++)
        {
            if (CheckSmelter(i))
            {
                smelters[i].time -= 1;
                if (smelters[i].time < 1)
                {
                    FinishSmelter(i);
                    i--;
                }
            }
            else i--;
        }
    }

    public static void RegisterSmelter(Smelter smelter)
    {
        smelters.Add(new ActiveSmelters(smelter, smelter.recipe.time));
    }

    private void FinishSmelter(int i)
    {

    }

    private bool CheckSmelter(int i)
    {
        if (smelters[i].smelter == null)
        {
            smelters.RemoveAt(i);
            return false;
        }
        return true;
    }
}
