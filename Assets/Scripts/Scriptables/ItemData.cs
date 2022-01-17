using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class ItemData : EntityData
{
    public int maxStackSize;
    public List<Recipe> recipes;
}
