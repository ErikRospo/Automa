using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/Building")]
public class BuildingData : ItemData
{
    /// <summary>
    /// Used to set a cell in the <see cref="Grid"/> class
    /// </summary>
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    /// <summary>
    /// The game object containing the <see cref="Building"/> MonoBehaviour
    /// </summary>
    public GameObject obj;

    /// <summary>
    /// List of <see cref="Cell"/>s this building takes up in the <see cref="Grid"/> class
    /// </summary>
    public Cell[] cells;

    /// <summary>
    /// Horizontal offset for the hologram display in <see cref="BuildingController"/>
    /// </summary>
    public Vector2 horizontalOffset;

    /// <summary>
    /// Vertical offset for the hologram display in <see cref="BuildingController"/>
    /// </summary>
    public Vector2 verticalOffset;

    /// <summary>
    /// Flag to determine if the building should be axis locked placing
    /// </summary>
    public bool axisSnappable;

    /// <summary>
    /// Flag to determine if the building should be rotatable in <see cref="BuildingController"/>
    /// </summary>
    public bool rotatable;

    /// <summary>
    /// If not empty, building will only be placeable on the specified minerals
    /// </summary>
    public List<DepositData> spawnableOn;
}
