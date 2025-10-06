using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace HealthSystem
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private bool instantDeactivateAfterTrigger;
        [SerializeField] private HealthType[] damageTo;
        [SerializeField] private float deactivationTime;
        [field: SerializeField] public int Damage { get; private set; }

        private DamageSourceType _damageSourceType = DamageSourceType.DAMAGE_ZONE;
        private Collider2D _coll;

        public event Action<Transform> OnDamageGiven;

        private void Awake() => _coll = GetComponent<Collider2D>();

        public void SetDamage(int damage) => Damage = damage;

        public void SetDeactivationTime(float value) => deactivationTime = value;
        
        public void SetInstantDeactivateAfterTrigger(bool value) => instantDeactivateAfterTrigger = value;

        public void SetZoneActive(bool active, HealthType[] overwriteDamageTo = null)
        {
            if(overwriteDamageTo != null)
                damageTo = overwriteDamageTo;
            
            SetZoneState(active);
        }
        
        public void SetZoneActive(bool active, DamageSourceType overwriteSource, HealthType[] overwriteDamageTo = null)
        {
            _damageSourceType = overwriteSource;
            
            if(overwriteDamageTo != null)
                damageTo = overwriteDamageTo;

            SetZoneState(active);
        }

        private void SetZoneState(bool active)
        {
            gameObject.SetActive(active);
            _coll.enabled = active;

            if (active && deactivationTime > 0)
                StartCoroutine(TimeDeactivateCoroutine());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.transform.TryGetComponent(out IHealth health) || !damageTo.Contains(health.GetHealthType()))
                return;

            health.ChangeHealth(-Damage, _damageSourceType);

            if (instantDeactivateAfterTrigger)
                _coll.enabled = false;

            StopAllCoroutines();
            OnDamageGiven?.Invoke(other.transform);
        }

        private IEnumerator TimeDeactivateCoroutine()
        {
            yield return new WaitForSeconds(deactivationTime);

            _coll.enabled = false;
            gameObject.SetActive(false);
        }
    }
}