using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Machine", menuName = "Buildings/Crafter")]
public class Machine : Tile
{
    public List<Recipe> recipes;
}
