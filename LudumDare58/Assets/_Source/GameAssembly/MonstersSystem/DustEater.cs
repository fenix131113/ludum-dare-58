using EntitySystem.Entities;
using HealthSystem;
using UnityEngine;

namespace MonstersSystem
{
    public class DustEater : PatrolPathHealthEntity
    {
        private const DamageSourceType VULNERABLE_DAMAGE_SOURCE = DamageSourceType.FLUTE;

        [SerializeField] private MonsterVision vision;

        private bool _isDamaged;

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (damageSource != VULNERABLE_DAMAGE_SOURCE)
                return;

            base.ChangeHealth(health, damageSource);
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