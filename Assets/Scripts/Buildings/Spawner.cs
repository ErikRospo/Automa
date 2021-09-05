using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "New Spawner", menuName = "Buildings/Spawner")]
public class Spawner : Building
{
    int frameTracker = 0;
    int frameLimit = 1000;

    public Item item;

    public Vector2 facingTile;

    private void Start()
    {
        // Set rotation
        switch (transform.rotation.eulerAngles.z)
        {
            case 90f:
                facingTile = new Vector2(transform.position.x, transform.position.y + 5f);
                break;
            case 180f:
                facingTile = new Vector2(transform.position.x - 5f, transform.position.y);
                break;
            case 270f:
                facingTile = new Vector2(transform.position.x, transform.position.y - 5f);
                break;
            default:
                facingTile = new Vector2(transform.position.x + 5f, transform.position.y);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        frameTracker++;
        if (frameTracker == frameLimit)
        {
            frameTracker = 0;
            Building building = BuildingHandler.active.TryGetBuilding(facingTile);
            if (building != null && building.acceptingEntities)
            {
                Entity lastEntity = EntityHandler.RegisterEntity(item, transform.position, Quaternion.identity);
                if (!building.PassEntity(lastEntity))
                    Destroy(lastEntity.transform);
            }
        }
    }


}
