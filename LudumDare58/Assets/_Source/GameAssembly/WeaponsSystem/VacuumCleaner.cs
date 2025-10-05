using System.Linq;
using Core;
using HealthSystem;
using ItemsSystem.Data;
using PlayerSystem;
using UnityEngine;
using VContainer;

namespace WeaponsSystem
{
    public class VacuumCleaner : APlayerHandItem
    {
        [SerializeField] private float overheatEdge;
        [SerializeField] private WeaponItemDataSO data;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;
        [Inject] private PlayerAim _playerAim;

        private float _lastShotTime;
        private float _shootingTime;
        private float _overheatedTime;
        private bool _overHeated;

        private void Update()
        {
            if (!gameObject.activeSelf || !_gameVariables.CanUseItems)
                return;

            if (_overHeated)
            {
                if (Time.time - _overheatedTime >= data.ReloadTime)
                {
                    _overHeated = false;
                    _shootingTime = 0;
                }
                else
                    return;
            }

            if (_input.Player.Attack.IsPressed())
            {
                _shootingTime += Time.deltaTime;

                if (_lastShotTime == 0 || Time.time - _lastShotTime >= data.ShootIntervalTime)
                {
                    _lastShotTime = Time.time;
                    Shoot();
                }
            }
            else if (!_overHeated && _shootingTime > 0)
                _shootingTime -= Time.deltaTime * 1.75f;

            if (_shootingTime < overheatEdge)
                return;

            _overheatedTime = Time.time;
            _overHeated = true;
        }

        private void Shoot()
        {
            var direction = new Vector2(Mathf.Cos(_playerAim.RotateAngle * Mathf.Deg2Rad),
                Mathf.Sin(_playerAim.RotateAngle * Mathf.Deg2Rad));
            
            var hit = Physics2D.Raycast(transform.position, direction, data.AttackDistance);
            
            if(!hit)
                return;

            var health = hit.transform.GetComponent<IHealth>();
            if(health == null || !data.DamageTo.Contains(health.GetHealthType()))
                return;
            
            health.ChangeHealth(-data.Damage, DamageSourceType.VACUUM_CLEANER);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_playerAim || !data)
                return;

            var direction = new Vector2(Mathf.Cos(transform.parent.rotation.z * Mathf.Deg2Rad),
                Mathf.Sin(transform.parent.rotation.z * _playerAim.RotateAngle * Mathf.Deg2Rad));
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, direction * data.AttackDistance);
        }
#endif
    }
}