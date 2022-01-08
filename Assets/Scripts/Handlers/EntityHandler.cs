using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHandler : NetworkBehaviour
{
    public class MovingEntity
    {
        public MovingEntity(float speed, Vector2 target, ItemEntity entity, Building building, bool output = false)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
            this.building = building;
            this.output = output;
        }

        public float speed;
        public Vector2 target;
        public ItemEntity entity;
        public Building building;
        public bool output;
    }
    public List<MovingEntity> movingEntities = new List<MovingEntity>();

    public class MovingObject
    {
        public MovingObject(BaseObject obj, float speed, Vector2 target, Transform transform = null)
        {
            this.obj = obj;
            this.speed = speed;
            this.target = target;

            if (transform == null) this.transform = obj.transform;
            else this.transform = obj.transform;
        }

        public BaseObject obj;
        public float speed;
        public Vector2 target;
        public Transform transform;
    }
    public List<MovingObject> movingObjects = new List<MovingObject>();

    public Transform entityContainer;

    public static EntityHandler active;

    public void Start()
    {
        if (this != null)
            active = this;
    }

    private void Update()
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

        for (int i = 0; i < movingObjects.Count; i++)
        {
            pos = movingObjects[i].transform;
            pos.position = Vector2.MoveTowards(pos.position, movingObjects[i].target, movingObjects[i].speed * Time.deltaTime);
            if (pos.position.x == movingObjects[i].target.x && pos.position.y == movingObjects[i].target.y)
            {
                RemoveMovingObject(movingObjects[i]);
                i--;
            }
        }
    }

    // Registers a new conveyor entity and returns it to the callign script
    public void RegisterMovingEntity(float speed, Vector2 target, ItemEntity entity, Building building, bool output = false)
    {
        MovingEntity newEntity = new MovingEntity(speed, target, entity, building, output);
        movingEntities.Add(newEntity);
    }

    // Registers a new moving object (can be anything)
    public MovingObject RegisterMovingObject(BaseObject obj, float speed, Vector2 target, Transform transform = null)
    {
        MovingObject newObject = new MovingObject(obj, speed, target, transform);
        movingObjects.Add(newObject);
        return newObject;
    }

    // Removes a conveyor entity
    public void RemoveMovingEntity(MovingEntity entity)
    {
        if (entity.building == null) Recycler.AddRecyclable(entity.entity.transform);
        else if (entity.output) entity.building.OutputEntity(entity.entity);
        else entity.building.ReceiveEntity(entity.entity);
        movingEntities.Remove(entity);
    }

    // Removes a conveyor entity
    public void RemoveMovingObject(MovingObject obj)
    {
        if (obj.obj != null) obj.obj.FinishMoving();
        movingObjects.Remove(obj);
    }

    public ItemEntity RegisterEntity(ItemData item, Vector2 position, Quaternion rotation)
    {
        Transform obj = Instantiate(entityContainer, position, rotation);

        ItemEntity lastEntity = obj.GetComponent<ItemEntity>();
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

        lastEntity.item = item;
        lastEntity.name = item.name;
        sprite.sprite = item.icon;
        return lastEntity;
    }

}
