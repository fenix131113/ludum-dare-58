using EntitySystem.Entities;
using HealthSystem;
using UnityEngine;

namespace MonstersSystem
{
    public class PatrolMonster : PatrolPathHealthEntity
    {
        private static readonly int _isMovingKey = Animator.StringToHash("IsMoving");
        
        [SerializeField] private bool runAfterDamage;
        [SerializeField] private MonsterVision vision;
        [SerializeField] private DamageSourceType vulnerableDamageSource;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MonsterJar jarPrefab;

        private bool _isDamaged;

        protected override void Update()
        {
            base.Update();
            
            spriteRenderer.flipX = path.velocity.x > 0;
            anim.SetBool(_isMovingKey, path.velocity != Vector3.zero);
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
            if(!runAfterDamage)
            {
                MoveToFarthestPoint();
                return;
            }

            if (_isDamaged)
                MoveToFarthestPoint();
            else
                StopMoving();
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
            vision.OnTargetSpotted += OnPlayerSpotted;
            vision.OnTargetLost += OnPlayerLost;
            OnHealthChanged += OnHealthChangedEvent;
        }

        protected override void Expose()
        {
            vision.OnTargetSpotted -= OnPlayerSpotted;
            vision.OnTargetLost -= OnPlayerLost;
            OnHealthChanged -= OnHealthChangedEvent;
        }
    }
}