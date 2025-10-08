using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HealthSystem;
using UnityEngine;

namespace MonstersSystem.View
{
    public class MonsterHealthView : AHealthView
    {
        [SerializeField] private int healthPerHpUnit;
        [SerializeField] private Transform hpUnitsContainer;
        [SerializeField] private SpriteRenderer monsterRenderer;
        [SerializeField] private HealthUnit hpUnitPrefab;
        [SerializeField] private Color damageColor;
        [SerializeField] private float hitColorTime;
        [SerializeField] private float deactivateTime;

        private readonly List<HealthUnit> _healthUnits = new();

        private Color _startColor;

        protected override void Start()
        {
            base.Start();
            _startColor = monsterRenderer.color;
            InitializeHpUnits();
        }

        protected override void Draw(int oldValue, int newValue)
        {
            if (newValue < oldValue)
            {
                monsterRenderer.DOColor(damageColor, hitColorTime / 2).onComplete +=
                    () => monsterRenderer.DOColor(_startColor, hitColorTime / 2);
                
                hpUnitsContainer.gameObject.SetActive(true);
                StartCoroutine(DeactivateHealthPoints());
            }

            var temp = healthEntity.GetMaxHealth() - newValue;

            foreach (var t in _healthUnits)
            {
                if (temp >= healthPerHpUnit)
                {
                    t.SetDamage(healthPerHpUnit);
                    temp -= healthPerHpUnit;
                }
                else
                {
                    t.SetDamage(temp);
                    temp = 0;
                }

                if (temp == 0)
                    return;
            }
        }

        private void InitializeHpUnits()
        {
#if UNITY_EDITOR
            if (healthEntity.GetMaxHealth() % healthPerHpUnit != 0)
                Debug.LogWarning(
                    $"Health per unit not compare to max health. HpPerUnit: {healthPerHpUnit}, MaxHealth: {healthEntity.GetMaxHealth()}");
#endif

            for (var i = 0; i < healthEntity.GetMaxHealth() / healthPerHpUnit; i++)
            {
                var spawned = Instantiate(hpUnitPrefab, hpUnitsContainer);
                spawned.Initialize(healthPerHpUnit);
                _healthUnits.Add(spawned);
            }
        }

        private IEnumerator DeactivateHealthPoints()
        {
            yield return new WaitForSeconds(deactivateTime);
            
            hpUnitsContainer.gameObject.SetActive(false);
        }
    }
}