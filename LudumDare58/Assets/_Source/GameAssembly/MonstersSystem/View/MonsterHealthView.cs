using System.Collections.Generic;
using HealthSystem;
using UnityEngine;

namespace MonstersSystem.View
{
    public class MonsterHealthView : AHealthView
    {
        [SerializeField] private int healthPerHpUnit;
        [SerializeField] private Transform hpUnitsContainer;
        [SerializeField] private HealthUnit hpUnitPrefab;

        private readonly List<HealthUnit> _healthUnits = new();

        protected override void Start()
        {
            base.Start();
            InitializeHpUnits();
        }

        protected override void Draw(int oldValue, int newValue)
        {
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
                
                if(temp == 0)
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
    }
}