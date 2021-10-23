using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "World/Resource")]
public class Resource : WorldTile
{
    public new string name; // Name of the world tile 
    public Item item; // Item that will be given when mined
    public bool canSpawnOnLand; // Tells generator if resource can spawn on land
    public bool canSpawnOnLiquid; // Tells generator if resource can spawn on liquid
    public bool randomizeRotation; // Tells generator if resource can be rotated
}
