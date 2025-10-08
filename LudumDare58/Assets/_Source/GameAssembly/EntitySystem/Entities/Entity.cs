using EntitySystem.Data;
using EntitySystem.Entities.Interfaces;
using UnityEngine;

namespace EntitySystem.Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Start() => Bind();

        protected void OnDestroy() => Expose();

        public virtual void Configure(EntityConfigSO config)
        {
            if (TryGetComponent(out Collider2D coll))
                coll.isTrigger = config.TriggerCollision;
            if (config.PhysicEntity && !coll.GetComponent<Rigidbody2D>())
                gameObject.AddComponent<Rigidbody2D>();
        }

        public void Teleport(Transform target) => Teleport(target.position);

        public virtual void Teleport(Vector3 position) => transform.position = position;

        protected virtual void Bind()
        {
        }

        protected virtual void Expose()
        {
        }
    }
}