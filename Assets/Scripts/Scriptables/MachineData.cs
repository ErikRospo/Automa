using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Machine", menuName = "Buildings/Crafter")]
public class MachineData : BuildingData
{
    public List<Recipe> recipes;
}
