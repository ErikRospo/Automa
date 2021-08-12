using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "New Spawner", menuName = "Buildings/Spawner")]
public class Spawner : NetworkBehaviour
{
    int frameTracker = 0;
    int frameLimit = 1000;

    Entity entity;

    // Update is called once per frame
    void Update()
    {
        frameTracker++;
        if (frameTracker == frameLimit)
        {
            Debug.Log("Attempting to spawn item");
            Building building = BuildingHandler.TryGetBuilding(new Vector2(transform.position.x + 5f, transform.position.y));
            if (building != null)
            {
                Entity lastEntity = Instantiate(entity.transform, transform.position, Quaternion.identity).GetComponent<Entity>();
                lastEntity.name = entity.name;
                building.PassEntity(lastEntity);
            }
            frameTracker = 0;
        }
    }
}
