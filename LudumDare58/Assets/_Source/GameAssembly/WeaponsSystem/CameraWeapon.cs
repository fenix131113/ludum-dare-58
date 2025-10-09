using Core;
using HealthSystem;
using ItemsSystem.Data;
using PlayerSystem;
using PlayerSystem.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

namespace WeaponsSystem
{
    public class CameraWeapon : APlayerHandItem
    {
        [SerializeField] private DamageZone cameraDamageZone;
        [SerializeField] private WeaponItemDataSO data;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;

        private float _nextShotTime;
        
        private void Awake() => cameraDamageZone.SetDamage(data.Damage);

        public override void Activate()
        {
            base.Activate();
            Bind();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Expose();
        }

        private void OnAttackInput(InputAction.CallbackContext context)
        {
            if (!_gameVariables.CanUseItems || !_input.Player.enabled)
                return;
            
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Time.time >= _nextShotTime || _nextShotTime == 0)
            {
                _nextShotTime = Time.time + data.ShootIntervalTime;
                Attack();
            }
        }

        private void Attack()
        {
            cameraDamageZone.SetZoneActive(true, DamageSourceType.CAMERA, data.DamageTo);
        }

        private void OnDamageZoneAttack(Transform _)
        {
            cameraDamageZone.SetZoneActive(false);
        }

        protected override void Bind()
        {
            base.Bind();
            _input.Player.Attack.performed += OnAttackInput;
            cameraDamageZone.OnDamageGiven += OnDamageZoneAttack;
        }

        protected override void Expose()
        {
            base.Expose();
            _input.Player.Attack.performed -= OnAttackInput;
            cameraDamageZone.OnDamageGiven -= OnDamageZoneAttack;
        }
    }
}