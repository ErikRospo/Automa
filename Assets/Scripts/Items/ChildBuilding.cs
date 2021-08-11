using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Child Building", menuName = "Items/Building (Child)")]
public class ChildBuilding : Item
{
    public Tile parent;
}
