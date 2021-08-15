using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHandler : NetworkBehaviour
{
    public class MovingEntity
    {
        public MovingEntity(float speed, Vector3 target, Entity entity, Building building)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
            this.building = building;
        }

        public float speed;
        public Vector3 target;
        public Entity entity;
        public Building building;
    }
    public static List<MovingEntity> movingEntities;

    private void Start()
    {
        movingEntities = new List<MovingEntity>();
    }

    private void Update()
    {
        for (int i = 0; i < movingEntities.Count; i++)
        {
            MovingEntity a = movingEntities[i];
            Transform pos = a.entity.transform;
            pos.position = Vector2.MoveTowards(pos.position, a.target, a.speed * Time.deltaTime);
            if (pos.position == a.target)
            {
                RemoveMovingEntity(a);
                i--;
            }
        }
    }

    // Registers a new conveyor entity and returns it to the callign script
    public static MovingEntity RegisterMovingEntity(float speed, Vector3 target, Entity entity, Building building)
    {
        MovingEntity newEntity = new MovingEntity(speed, target, entity, building);
        movingEntities.Add(newEntity);
        return newEntity;
    }

    // Removes a conveyor entity
    public static void RemoveMovingEntity(MovingEntity entity)
    {
        entity.building.ReceiveEntity(entity.entity);
        movingEntities.Remove(entity);
    }

}
