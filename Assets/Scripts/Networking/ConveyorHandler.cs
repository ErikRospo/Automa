using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorHandler : MonoBehaviour
{
    public class ConveyorEntity
    {
        public ConveyorEntity(float speed, Vector3 target, Transform entity)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
        }

        public float speed;
        public Vector3 target;
        public Transform entity;
    }
    public static List<ConveyorEntity> conveyorEntities;

    private void Start()
    {
        conveyorEntities = new List<ConveyorEntity>();
    }

    private void Update()
    {
        for (int i = 0; i < conveyorEntities.Count; i++)
        {
            ConveyorEntity a = conveyorEntities[i];
            a.entity.position = Vector2.MoveTowards(a.entity.position, a.target, a.speed * Time.deltaTime);
            if (a.entity.position == a.target)
            {
                RemoveConveyorEntity(a);
                i--;
            }
        }
    }

    // Registers a new conveyor entity and returns it to the callign script
    public static ConveyorEntity RegisterConveyorEntity(float speed, Vector3 target, Transform entity)
    {
        ConveyorEntity newEntity = new ConveyorEntity(speed, target, entity);
        conveyorEntities.Add(newEntity);
        return newEntity;
    }

    // Removes a conveyor entity
    public static void RemoveConveyorEntity(ConveyorEntity entity)
    {
        conveyorEntities.Remove(entity);
    }

}
