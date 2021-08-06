using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ParentBuilding : Item
{
    public List<ChildBuilding> tiles;

    public void DestroyBuilding()
    {
        Tilemap grid = BuildingHandler.active.grid;

    }



}
