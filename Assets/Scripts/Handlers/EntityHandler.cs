using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHandler : NetworkBehaviour
{
    public class MovingEntity
    {
        public MovingEntity(float speed, Vector2 target, Entity entity, Building building, bool output = false)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
            this.building = building;
            this.output = output;
        }

        public float speed;
        public Vector2 target;
        public Entity entity;
        public Building building;
        public bool output;
    }
    public List<MovingEntity> movingEntities = new List<MovingEntity>();
    public Transform entityContainer;

    public static EntityHandler active;

    public void Start()
    {
        if (this != null)
            active = this;
    }
    private void FixedUpdate()
    {
        Transform pos;

        for (int i = 0; i < movingEntities.Count; i++)
        {
            pos = movingEntities[i].entity.transform;
            pos.position = Vector2.MoveTowards(pos.position, movingEntities[i].target, movingEntities[i].speed * Time.deltaTime);
            if (pos.position.x == movingEntities[i].target.x && pos.position.y == movingEntities[i].target.y)
            {
                RemoveMovingEntity(movingEntities[i]);
                i--;
            }
        }
    }

    // Registers a new conveyor entity and returns it to the callign script
    public void RegisterMovingEntity(float speed, Vector2 target, Entity entity, Building building, bool output = false)
    {
        MovingEntity newEntity = new MovingEntity(speed, target, entity, building, output);
        movingEntities.Add(newEntity);
    }

    // Removes a conveyor entity
    public void RemoveMovingEntity(MovingEntity entity)
    {
        if (entity.building == null) Recycler.AddRecyclable(entity.entity.transform);
        else if (entity.output) entity.building.OutputEntity(entity.entity);
        else entity.building.ReceiveEntity(entity.entity);
        movingEntities.Remove(entity);
    }

    public Entity RegisterEntity(Item item, Vector2 position, Quaternion rotation)
    {
        Transform obj = Instantiate(entityContainer, position, rotation);

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
            Debug.LogError("The entity " + item.name + " does not contain an image in the resources folder!");
            Recycler.AddRecyclable(obj);
            return null;
        }

        lastEntity.item = item;
        lastEntity.name = item.name;
        sprite.sprite = img;
        return lastEntity;
    }

}
