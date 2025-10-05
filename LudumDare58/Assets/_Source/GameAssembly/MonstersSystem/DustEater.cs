using EntitySystem.Entities;
using HealthSystem;
using PlayerSystem;

namespace MonstersSystem
{
    public class DustEater : PathHealthEntity
    {
        private const DamageSourceType VULNERABLE_DAMAGE_SOURCE = DamageSourceType.CAMERA;

        private PathHealthEntity _entity;

        protected override void Awake()
        {
            base.Awake();
            _entity = GetComponent<PathHealthEntity>();
        }

        protected override void Start()
        {
            _entity.SetTarget(FindFirstObjectByType<PlayerMovement>().transform);
        }

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (damageSource != VULNERABLE_DAMAGE_SOURCE)
                return;

            base.ChangeHealth(health, damageSource);
        }
    }
}