using System;
using HealthSystem;
using MonstersSystem.Data;
using Pathfinding;
using UnityEngine;
using Utils;
using VContainer;
using Random = UnityEngine.Random;

namespace EntitySystem.Entities
{
    public class PathHealthEntity : HealthEntity
    {
        [SerializeField] protected TempAudioSource deathSoundSourcePrefab;
        [SerializeField] protected AIPath path;
        [SerializeField] protected AIDestinationSetter destinationSetter;

        [Inject] protected MonstersSoundsPackSO SoundsPack;

        protected bool _isNativeDestinationReached;

        public event Action OnNativePathReached;

        protected virtual void Update()
        {
            if (!_isNativeDestinationReached && path.reachedEndOfPath)
            {
                var distance = Vector3.Distance(transform.position, destinationSetter.target.position);

                if (path.reachedEndOfPath && distance <= path.endReachedDistance)
                    OnNativeDestinationReached();
            }
        }

        protected virtual void OnNativeDestinationReached()
        {
            destinationSetter.target = null;
            _isNativeDestinationReached = true;
            OnNativePathReached?.Invoke();
        }

        public virtual void SetTarget(Transform target)
        {
            destinationSetter.target = target;
            _isNativeDestinationReached = false;
        }

        protected override void Death()
        {
            Instantiate(deathSoundSourcePrefab)
                .PlayAndDestroy(SoundsPack.DeathSounds[Random.Range(0, SoundsPack.DeathSounds.Length)]);
            base.Death();
        }

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (!takeAnyDamage && damageSource != vulnerableDamageSource)
                return;

            if (GetHealth() + health < GetHealth() && GetHealth() + health > 0)
                Instantiate(deathSoundSourcePrefab)
                    .PlayAndDestroy(SoundsPack.DamageSounds[Random.Range(0, SoundsPack.DamageSounds.Length)]);

            base.ChangeHealth(health, damageSource);
        }

        public virtual void StopMoving() => path.isStopped = true;

        public virtual void ResumeMoving() => path.isStopped = false;
    }
}