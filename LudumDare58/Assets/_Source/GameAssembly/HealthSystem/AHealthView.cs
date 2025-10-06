using EntitySystem.Entities;
using UnityEngine;

namespace HealthSystem
{
    public abstract class AHealthView : MonoBehaviour
    {
        [SerializeField] protected HealthEntity healthEntity;

        protected virtual void Start() => Bind();

        protected virtual void OnDestroy() => Expose();

        protected virtual void OnHealthChanged(int oldValue, int newValue) => Draw(oldValue, newValue);

        protected abstract void Draw(int oldValue, int newValue);

        protected virtual void Bind() => healthEntity.OnHealthChanged += OnHealthChanged;

        protected virtual void Expose() => healthEntity.OnHealthChanged -= OnHealthChanged;
    }
}