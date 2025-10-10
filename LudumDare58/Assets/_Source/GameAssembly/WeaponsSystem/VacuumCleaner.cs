using System.Linq;
using Core;
using Core.Data;
using HealthSystem;
using ItemsSystem.Data;
using PlayerSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace WeaponsSystem
{
    public class VacuumCleaner : APlayerHandItem
    {
        [SerializeField] private float overheatEdge;
        [SerializeField] private GameObject vfx;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;
        [Inject] private PlayerAim _playerAim;
        [Inject] private LayersDataSO _layersData;

        private WeaponItemDataSO Data => ItemData as WeaponItemDataSO;
        private float _lastShotTime;
        private float _shootingTime;
        private float _overheatedTime;
        private bool _overHeated;

        private int
            _additionalDamage; //TODO: Make upgrade abstract system, maybe through IUpgradable or AUpgradeHandItem

        private void Update()
        {
            if (!gameObject.activeSelf || !_gameVariables.CanUseItems || !_input.Player.enabled)
            {
                vfx.SetActive(false);
                return;
            }

            if (_overHeated)
            {
                vfx.SetActive(false);
                if (Time.time - _overheatedTime >= Data.ReloadTime)
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

                if (_lastShotTime == 0 || Time.time - _lastShotTime >= Data.ShootIntervalTime)
                {
                    _lastShotTime = Time.time;
                    Shoot();
                }
            }
            else if (!_overHeated && _shootingTime > 0)
            {
                _shootingTime -= Time.deltaTime * 1.75f;
                vfx.SetActive(false);
            }
            else
                vfx.SetActive(false);

            if (_shootingTime < overheatEdge)
                return;

            _overheatedTime = Time.time;
            _overHeated = true;
        }

        private void Shoot()
        {
            if (EventSystem.current &&
                EventSystem.current.IsPointerOverGameObject()) //TODO: Make abstraction for hand weapons
                return;

            vfx.SetActive(true);

            var dir = new Vector2(
                Mathf.Cos(_playerAim.RotateAngle * Mathf.Deg2Rad),
                Mathf.Sin(_playerAim.RotateAngle * Mathf.Deg2Rad)
            );

            var hit = Physics2D.Raycast(transform.position, dir, Data.AttackDistance,
                ~_layersData.PlayerLayer & ~_layersData.IgnoreRaycast);

            if (!hit)
                return;

            var health = hit.transform.GetComponent<IHealth>();
            if (health == null || !Data.DamageTo.Contains(health.GetHealthType()))
                return;

            health.ChangeHealth(-(Data.Damage + _additionalDamage), DamageSourceType.VACUUM_CLEANER);
        }

        public void SetAdditionalDamage(int value) => _additionalDamage = value;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_playerAim || !Data)
                return;

            var dir = new Vector2(
                Mathf.Cos(_playerAim.RotateAngle * Mathf.Deg2Rad),
                Mathf.Sin(_playerAim.RotateAngle * Mathf.Deg2Rad)
            );

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, dir * Data.AttackDistance);
        }
#endif
    }
}