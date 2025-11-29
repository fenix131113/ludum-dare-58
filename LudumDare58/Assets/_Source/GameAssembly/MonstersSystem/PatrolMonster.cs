using System;
using EntitySystem.Entities;
using HealthSystem;
using UnityEngine;

namespace MonstersSystem
{
    public class PatrolMonster : PatrolPathHealthEntity
    {
        private static readonly int _isMovingKey = Animator.StringToHash("IsMoving");

        [SerializeField] private bool runAfterDamage;
        [SerializeField] private bool runWhenSpotted;
        [SerializeField] private MonsterVision vision;
        [SerializeField] private MonsterVision panicVision;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MonsterJar jarPrefab;

        [Header("Boost Settings")] [SerializeField]
        protected float speedBoost;

        [SerializeField] protected float boostDuration;
        [SerializeField] protected float boostCooldown;


        [SerializeField] protected float escapeOffset = 5f;

        private bool _isDamaged;
        protected bool _isBoosted;

        protected float _currentBoostDuration;
        protected float _currentBoostCooldown;


        protected override void Update()
        {
            base.Update();

            _currentBoostCooldown = Math.Max(_currentBoostCooldown, 0) - Time.deltaTime;
            _currentBoostDuration = Math.Max(_currentBoostDuration, 0) - Time.deltaTime;

            UnBoost();

            spriteRenderer.flipX = path.velocity.x > 0;
            anim.SetBool(_isMovingKey, path.velocity != Vector3.zero);
        }

        protected void Boost()
        {
            if (IsOnBoost() || IsBoostOnCooldown())
                return;
            
            _currentBoostDuration = boostDuration;
            path.maxSpeed += speedBoost;
            _isBoosted = true;
        }

        protected void UnBoost()
        {
            if (!IsOnBoost() && _isBoosted)
            {
                _currentBoostCooldown = boostCooldown;
                path.maxSpeed -= speedBoost;
                _isBoosted = false;
            }
        }

        protected bool IsOnBoost()
        {
            if (_currentBoostDuration > 0)
                return true;
            
            return false;
        }

        protected bool IsBoostOnCooldown()
        {
            if (_currentBoostCooldown > 0)
                return true;
            
            return false;
        }

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (damageSource != vulnerableDamageSource)
                return;

            base.ChangeHealth(health, damageSource);
        }

        protected override void Death()
        {
            base.Death();
            Instantiate(jarPrefab, transform.position, Quaternion.identity);
        }

        protected override void OnNativeDestinationReached()
        {
            base.OnNativeDestinationReached();

            if (!IsPatrol && vision.CanSeeTarget)
            {
                MoveToFarthestPoint();
            }
        }

        private void OnPlayerSpotted()
        {
            if (runAfterDamage)
            {
                if (_isDamaged)
                    MoveToFarthestPoint();
                else
                    StopMoving();
                return;
            }

            if (runWhenSpotted)
            {
                var direction = transform.position - vision.Target.position;
                direction.Normalize();
                direction *= vision.gameObject.GetComponent<CircleCollider2D>().radius + escapeOffset;
                path.destination = vision.Target.position + direction;
                MoveToFarthestPoint();
            }
        }

        private void OnPlayerLost()
        {
            ResumeMoving();
            StartPatrol();
        }

        private void OnHealthChangedEvent(int oldValue, int newValue)
        {
            if (newValue >= oldValue)
                return;

            _isDamaged = true;
            MoveToFarthestPoint();
        }

        protected override void Bind()
        {
            base.Bind();
            vision.OnTargetSpotted += OnPlayerSpotted;
            vision.OnTargetLost += OnPlayerLost;
            panicVision.OnTargetSpotted += Boost;
            OnHealthChanged += OnHealthChangedEvent;
        }

        protected override void Expose()
        {
            base.Expose();
            vision.OnTargetSpotted -= OnPlayerSpotted;
            vision.OnTargetLost -= OnPlayerLost;
            panicVision.OnTargetSpotted -= Boost;
            OnHealthChanged -= OnHealthChangedEvent;
        }
    }
}