using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EntityHandler : NetworkBehaviour
{
    public class MovingEntity
    {
        public MovingEntity(float speed, Vector3 target, Entity entity, Building building, bool output = false)
        {
            this.speed = speed;
            this.target = target;
            this.entity = entity;
            this.building = building;
            this.output = output;
        }

        public float speed;
        public Vector3 target;
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
    public void RegisterMovingEntity(float speed, Vector3 target, Entity entity, Building building, bool output = false)
    {
        MovingEntity newEntity = new MovingEntity(speed, target, entity, building, output);
        movingEntities.Add(newEntity);
    }

    // Removes a conveyor entity
    public void RemoveMovingEntity(MovingEntity entity)
    {
        if (entity.output) entity.building.OutputEntity(entity.entity);
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
