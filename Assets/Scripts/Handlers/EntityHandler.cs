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

    public static Transform _entityContainer;
    public Transform entityContainer;

    private void Start()
    {
        _entityContainer = entityContainer;
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
    public static void RegisterMovingEntity(float speed, Vector3 target, Entity entity, Building building)
    {
        MovingEntity newEntity = new MovingEntity(speed, target, entity, building);
        movingEntities.Add(newEntity);
    }

    // Removes a conveyor entity
    public static void RemoveMovingEntity(MovingEntity entity)
    {
        entity.building.ReceiveEntity(entity.entity);
        movingEntities.Remove(entity);
    }

    public static Entity RegisterEntity(Item item, Vector2 position, Quaternion rotation)
    {
        Transform obj = Instantiate(_entityContainer, position, rotation);

        Entity lastEntity = obj.GetComponent<Entity>();
        if (lastEntity == null)
        {
            Debug.LogError("The entity you tried to create does not contain an entity script!");
            Recycler.AddRecyclable(obj);
            return null;
        }

        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("The entity you tried to create does not contain a sprite renderer!");
            Recycler.AddRecyclable(obj);
            return null;
        }

        Sprite img = Resources.Load<Sprite>("Sprites/Items/" + item.name);
        if (img == null)
        {
            Debug.LogError("The entity you tried to create does not contain an image in the resources folder!");
            Recycler.AddRecyclable(obj);
            return null;
        }

        lastEntity.item = item;
        lastEntity.name = item.name;
        sprite.sprite = img;
        return lastEntity;
    }

}
