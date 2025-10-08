using System;
using EntitySystem.Entities;
using HealthSystem;
using PlayerSystem;
using UnityEngine;

namespace MonstersSystem.Boss
{
    public class Boss : PathHealthEntity
    {
        private static readonly int _isMovingKey = Animator.StringToHash("IsMoving");
        [SerializeField] private MonsterJar jarPrefab;
        [SerializeField] private MonsterVision vision;
        [SerializeField] private Animator anim;
        [SerializeField] private BossStage[] stages;
        [SerializeField] private Transform[] movePoints;

        private Transform _player;
        private int _currentStage;
        private int _givenDamageCurrentStage;

        protected override void Start()
        {
            _player = FindFirstObjectByType<PlayerMovement>().transform;
            base.Start();
            ChangeStage(_currentStage);
        }

        protected override void Update()
        {
            base.Update();
            anim.SetBool(_isMovingKey, path.velocity != Vector3.zero);
        }

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (stages[_currentStage].vulnerableDamageType != damageSource || stages[_currentStage].noDamage)
                return;

            var temp = health;
            health = Mathf.Clamp(health, -(stages[_currentStage].damageToNextStage - _givenDamageCurrentStage), 0);
            _givenDamageCurrentStage -= temp;
            base.ChangeHealth(health, damageSource);
            
            if (_givenDamageCurrentStage >= stages[_currentStage].damageToNextStage)
                ChangeStage(_currentStage + 1);
        }

        private void ChangeStage(int stage)
        {
            if (stage >= stages.Length)
                return;

            _currentStage = stage;
            switch (stages[_currentStage].bossStageType)
            {
                case BossStageType.ZERO:
                    StopMoving();
                    break;
                case BossStageType.FIRST:
                    ResumeMoving();
                    SetTarget(_player);
                    break;
                case BossStageType.SECOND:
                    Teleport(GetFarthestPoint());
                    ResumeMoving();
                    break;
                case BossStageType.THIRD:
                    ResumeMoving();
                    SetTarget(_player);
                    break;
            }

            _givenDamageCurrentStage = 0;
        }

        protected override void Death()
        {
            Instantiate(jarPrefab, transform.position, Quaternion.identity);
            base.Death();
        }

        protected override void OnNativeDestinationReached() => destinationSetter.target = _player;

        private void OnTargetSpotted()
        {
            if (_currentStage == 0)
                ChangeStage(1);
        }

        public Transform GetFarthestPoint()
        {
            if (movePoints.Length == 0)
                return null;

            Transform farthest = null;

            foreach (var patrolPoint in movePoints)
            {
                if (!farthest)
                {
                    farthest = patrolPoint;
                    continue;
                }

                if (Vector2.Distance(transform.position, farthest.position) <
                    Vector2.Distance(transform.position, patrolPoint.position))
                    farthest = patrolPoint;
            }

            return farthest;
        }

        protected override void Bind()
        {
            vision.OnTargetSpotted += OnTargetSpotted;
        }

        protected override void Expose()
        {
            vision.OnTargetSpotted -= OnTargetSpotted;
        }

        [Serializable]
        public class BossStage
        {
            [SerializeField] public bool noDamage;
            [SerializeField] public BossStageType bossStageType;
            [SerializeField] public DamageSourceType vulnerableDamageType;
            [SerializeField] public int damageToNextStage;
        }
    }
}