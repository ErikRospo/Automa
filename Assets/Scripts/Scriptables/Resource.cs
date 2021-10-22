using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "World/Resource")]
public class Resource : WorldTile
{
    public Item item; // Item that will be given when mined
}
