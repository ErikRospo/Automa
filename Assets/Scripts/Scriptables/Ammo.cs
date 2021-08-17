using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Items/Ammo")]
public class Ammo : Item
{
    public int damage;
    public int clipSize;
}
