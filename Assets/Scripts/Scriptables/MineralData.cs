using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "World/Resource")]
public class MineralData : WorldTile
{
    public EntityData item; // Item that will be given when mined
    public bool canSpawnOnLand; // Tells generator if resource can spawn on land
    public bool canSpawnOnLiquid; // Tells generator if resource can spawn on liquid
    public bool randomizeRotation; // Tells generator if resource can be rotated
}
