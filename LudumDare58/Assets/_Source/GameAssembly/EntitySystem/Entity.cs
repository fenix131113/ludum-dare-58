using EntitySystem.Data;
using UnityEngine;

namespace EntitySystem
{
    public class Entity : MonoBehaviour, IEntity
    {
        public virtual void Configure(EntityConfigSO config)
        {
            if (TryGetComponent(out Collider2D coll))
                coll.isTrigger = config.TriggerCollision;
            if (config.PhysicEntity && !coll.GetComponent<Rigidbody2D>())
                gameObject.AddComponent<Rigidbody2D>();
        }
    }
}