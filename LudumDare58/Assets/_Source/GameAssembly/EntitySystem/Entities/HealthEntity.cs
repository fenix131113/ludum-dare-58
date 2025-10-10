using System;
using EntitySystem.Entities.Interfaces;
using HealthSystem;
using UnityEngine;

namespace EntitySystem.Entities
{
    public class HealthEntity : Entity, IHealthEntity
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected HealthType healthType;
        [SerializeField] protected bool takeAnyDamage = true;
        [SerializeField] protected DamageSourceType vulnerableDamageSource;
        
        protected int _health;
        
        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        protected virtual void Awake()
        {
            _health = maxHealth;
        }

        public Entity GetEntity() => this;

        public HealthType GetHealthType() => healthType;

        public virtual int GetHealth() => _health;

        public virtual int GetMaxHealth() => maxHealth;

        protected virtual void Death()
        {
            gameObject.SetActive(false);
            Expose();
        }

        public virtual void ChangeHealth(int health,  DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if(!takeAnyDamage && damageSource != vulnerableDamageSource)
                return;
            
            var temp = _health;
            _health = Mathf.Clamp(_health + health, 0, maxHealth);
            
            OnHealthChanged?.Invoke(temp, _health);
            
            if(_health == 0)
            {
                OnDeath?.Invoke();
                Death();
            }
        }
    }
}