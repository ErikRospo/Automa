using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ConveyorHandler : NetworkBehaviour
{
    public class ConveyorEntity
    {
        public ConveyorEntity(float speed, Vector3 target, Entity entity, Conveyor conveyor)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
            obj = entity.transform;
            this.conveyor = conveyor;
        }

        public float speed;
        public Vector3 target;
        public Entity entity;
        public Transform obj;
        public Conveyor conveyor;
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
            a.obj.position = Vector2.MoveTowards(a.obj.position, a.target, a.speed * Time.deltaTime);
            if (a.obj.position == a.target)
            {
                RemoveConveyorEntity(a);
                i--;
            }
        }
    }

    // Registers a new conveyor entity and returns it to the callign script
    public static ConveyorEntity RegisterConveyorEntity(float speed, Vector3 target, Entity entity, Conveyor conveyor)
    {
        ConveyorEntity newEntity = new ConveyorEntity(speed, target, entity, conveyor);
        conveyorEntities.Add(newEntity);
        return newEntity;
    }

    // Removes a conveyor entity
    public static void RemoveConveyorEntity(ConveyorEntity entity)
    {
        entity.conveyor.SetBin(entity.entity);
        conveyorEntities.Remove(entity);
    }

}
