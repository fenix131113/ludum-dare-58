using Core;
using ItemsSystem.Data;
using PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace WeaponsSystem
{
    public class VacuumCleaner : APlayerHandItem
    {
        [SerializeField] private float overheatEdge;
        [SerializeField] private WeaponItemDataSO data;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;

        private float _lastShotTime;
        private float _shootingTime;
        private float _overheatedTime;
        private bool _overHeated;

        private void OnEnable() => Bind();

        private void OnDestroy() => Expose();

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
                _shootingTime -= Time.deltaTime;

            if (_shootingTime < overheatEdge)
                return;
            
            _overheatedTime = Time.time;
            _overHeated = true;
        }

        private void Shoot()
        {
            Debug.Log("Shoot");
        }

        private void OnAttackInput(InputAction.CallbackContext context)
        {
        }

        private void Bind() => _input.Player.Attack.performed += OnAttackInput;

        private void Expose() => _input.Player.Attack.performed -= OnAttackInput;
    }
}