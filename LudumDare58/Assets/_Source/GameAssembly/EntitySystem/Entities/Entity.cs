using EntitySystem.Data;
using EntitySystem.Entities.Interfaces;
using UnityEngine;

namespace EntitySystem.Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        protected virtual void Awake() // Created so as not to rewrite the code in all the heirs, if logic appears here.
        {
        }

        protected virtual void Start() => Bind();

        protected void OnDestroy() => Expose();

        protected void Update() // Created so as not to rewrite the code in all the heirs, if logic appears here.
        {
        }

        public virtual void Configure(EntityConfigSO config)
        {
            if (TryGetComponent(out Collider2D coll))
                coll.isTrigger = config.TriggerCollision;
            if (config.PhysicEntity && !coll.GetComponent<Rigidbody2D>())
                gameObject.AddComponent<Rigidbody2D>();
        }

        protected virtual void Bind()
        {
        }

        protected virtual void Expose()
        {
        }
    }
}