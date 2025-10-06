using System.Collections.Generic;
using System.Linq;
using Core;
using ItemsSystem.Data;
using PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;
using Random = UnityEngine.Random;

namespace WeaponsSystem
{
    public class Flute : APlayerHandItem
    {
        private static readonly char[] _possibleKeys = { 'Q', 'W', 'E', 'R', 'A', 'S', 'D', 'F', 'Z', 'X', 'C', 'V' };

        [SerializeField] private WeaponItemDataSO data;
        [SerializeField] private int keysCombinationCount;
        [SerializeField] private Image fluteButtonUiPrefab;
        [SerializeField] private Transform fluteButtonsContent;
        [SerializeField] private FluteCircle fluteCirclePrefab;

        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;

        private readonly List<Image> _uiButtons = new();

        private List<char> _currentCombination;
        private int _currentCombinationIndex;
        private float _nextShotTime;

        private void Update()
        {
            if (_currentCombination == null || !Keyboard.current.anyKey.wasPressedThisFrame)
                return;

            if (_currentCombination[_currentCombinationIndex].ToString().ToLower() == GetPressedKeyName())
                OnCorrectClick();
            else
                OnIncorrectClick();
        }

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

        private void OnCorrectClick()
        {
            if(_currentCombinationIndex >= _uiButtons.Count || _currentCombination == null)
                return;
            
            _uiButtons[_currentCombinationIndex].color = Color.darkGreen;

            _currentCombinationIndex++;
            if (_currentCombinationIndex == _currentCombination.Count) // Win mini-game
            {
                _currentCombination = null;
                SetAttackCooldown();
                ClearUi();
                _input.Player.Enable();
                AttackWave();
            }
        }

        private void AttackWave() => Instantiate(fluteCirclePrefab, transform.position, Quaternion.identity)
            .SizeUp(data.AttackDistance, data.Damage);

        private void OnIncorrectClick()
        {
            ClearUi();
            _input.Player.Enable();
            SetAttackCooldown();
            _currentCombination = null;
        }

        private string GetPressedKeyName()
        {
            foreach (var key in Keyboard.current.allKeys.Where(key => key.wasPressedThisFrame))
                return key.name;

            return string.Empty;
        }

        private void OnAttackInput(InputAction.CallbackContext context)
        {
            if (!gameObject.activeSelf || !_gameVariables.CanUseItems  || !_input.Player.enabled)
                return;

            if (_nextShotTime == 0 || Time.time >= _nextShotTime)
                Attack();
        }

        private void Attack()
        {
            _input.Player.Disable();
            _currentCombination = new List<char>();

            for (var i = 0; i < keysCombinationCount; i++)
                _currentCombination.Add(_possibleKeys[Random.Range(0, _possibleKeys.Length)]);

            _currentCombinationIndex = 0;
            InitializeUi();
        }
        
        private void SetAttackCooldown() => _nextShotTime = Time.time + data.ShootIntervalTime;

        private void InitializeUi()
        {
            if (_currentCombination == null)
                return;

            for (var i = 0; i < _currentCombination.Count; i++)
            {
                var spawned = Instantiate(fluteButtonUiPrefab, fluteButtonsContent);
                _uiButtons.Add(spawned);
                spawned.transform.GetChild(0).GetComponent<TMP_Text>().text = _currentCombination[i].ToString();
            }
        }

        private void ClearUi()
        {
            foreach (var button in _uiButtons)
                Destroy(button.gameObject); //TODO: change this to object pool

            _uiButtons.Clear();
        }

        protected override void Bind()
        {
            base.Bind();
            _input.Player.Attack.performed += OnAttackInput;
        }

        protected override void Expose()
        {
            base.Expose();
            _input.Player.Attack.performed -= OnAttackInput;
        }
    }
}