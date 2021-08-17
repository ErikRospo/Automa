using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Items/Gun")]
public class Gun : Item
{
    public int damage;
    public Ammo ammo;
}
