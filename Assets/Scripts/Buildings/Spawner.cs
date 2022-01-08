using UnityEngine;

public class Spawner : Building
{
    int frameTracker = 0;
    int frameLimit = 500;

    public ItemData item;

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
                ItemEntity lastEntity = EntityHandler.active.RegisterEntity(item, transform.position, Quaternion.identity);
                if (!building.InputEntity(lastEntity))
                    Destroy(lastEntity.transform);
            }
        }
    }
}