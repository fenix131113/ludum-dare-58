using System;
using System.Collections.Generic;
using DG.Tweening;
using EntitySystem.Entities;
using HealthSystem;
using PlayerSystem;
using UnityEngine;

namespace MonstersSystem
{
    public class Mimic : PathHealthEntity
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MonsterJar jarPrefab;
        [SerializeField] private float distanceToDamage;
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeStrength;
        [SerializeField] private GameObject startMimicObject;
        [SerializeField] private List<MimicPoint> points;

        private Transform _playerTransform;
        private int _currentPointIndex = -1;

        protected override void Start()
        {
            _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
            base.Start();
        }

        protected override void Death()
        {
            base.Death();
            Instantiate(jarPrefab, transform.position, Quaternion.identity);
        }

        public override void ChangeHealth(int health, DamageSourceType damageSource = DamageSourceType.UNKNOWN)
        {
            if (damageSource != vulnerableDamageSource)
                return;

            if (Vector2.Distance(_playerTransform.position, transform.position) > distanceToDamage)
            {
                Shake();
                return;
            }

            base.ChangeHealth(health, damageSource);

            startMimicObject.SetActive(false);
            TransformToMimic();

            if (_currentPointIndex + 1 < points.Count)
                _currentPointIndex++;
            else
                _currentPointIndex = 0;

            SetTarget(points[_currentPointIndex].Point);
        }

        public override void SetTarget(Transform target)
        {
            base.SetTarget(target);
            TransformToMimic();
        }

        private void TransformToCurrentObject()
        {
            spriteRenderer.enabled = false;
            points[_currentPointIndex].ObjectToTransform.SetActive(true);
        }

        private void TransformToMimic()
        {
            if (_currentPointIndex >= 0)
                points[_currentPointIndex].ObjectToTransform.SetActive(false);
            
            spriteRenderer.enabled = true;
        }

        private void Shake()
        {
            transform.DOShakePosition(shakeDuration, shakeStrength);
        }

        protected override void OnNativeDestinationReached()
        {
            TransformToCurrentObject();
            base.OnNativeDestinationReached();
        }

        protected override void Expose()
        {
            base.Expose();
            DOTween.Kill(this);
        }

        [Serializable]
        public class MimicPoint
        {
            [field: SerializeField] public Transform Point { get; private set; }
            [field: SerializeField] public GameObject ObjectToTransform { get; private set; }
        }
    }
}