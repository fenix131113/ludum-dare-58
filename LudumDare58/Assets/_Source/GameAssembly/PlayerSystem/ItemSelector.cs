using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using InventorySystem;
using PlayerSystem.View;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace PlayerSystem
{
    public class ItemSelector : IInitializable, ITickable
    {
        [Inject] private InputSystem_Actions _input;
        [Inject] private GameVariables _gameVariables;
        [Inject] private Inventory _inventory;
        [Inject] private PlayerInventoryView _playerInventoryView;

        public ItemCell SelectedCell { get; private set; }
        private int _currentSelectedIndex = -1;

        public void Initialize() => Bind();

        ~ItemSelector() => Expose();

        public event Action OnSelectedItemChanged;

        public void Tick()
        {
            if (!_gameVariables.CanChangeItems || _playerInventoryView.ActiveCells.Count == 0)
                return;

            if (Mouse.current.scroll.y.ReadValue() > 0)
            {
                if (_currentSelectedIndex == -1)
                {
                    _currentSelectedIndex = 0;
                    SelectCell(_currentSelectedIndex);
                    return;
                }

                if (_currentSelectedIndex + 1 == _playerInventoryView.ActiveCells.Count)
                    _currentSelectedIndex = 0;
                else
                    _currentSelectedIndex++;

                SelectCell(_currentSelectedIndex);
            }
            else if (Mouse.current.scroll.y.ReadValue() < 0)
            {
                if (_currentSelectedIndex == -1)
                {
                    _currentSelectedIndex = _playerInventoryView.ActiveCells.Count - 1;
                    SelectCell(_currentSelectedIndex);
                    return;
                }

                if (_currentSelectedIndex - 1 == -1)
                    _currentSelectedIndex = _playerInventoryView.ActiveCells.Count - 1;
                else
                    _currentSelectedIndex--;

                SelectCell(_currentSelectedIndex);
            }
        }

        private void OnSlotPerformed(InputAction.CallbackContext context)
        {
            if (!_gameVariables.CanChangeItems)
                return;

            var index = context.action.name switch
            {
                nameof(_input.Player.FirstSlot) => 0,
                nameof(_input.Player.SecondSlot) => 1,
                nameof(_input.Player.ThirdSlot) => 2,
                nameof(_input.Player.FourthSlot) => 3,
                nameof(_input.Player.FifthSlot) => 4,
                nameof(_input.Player.SixthSlot) => 5,
                nameof(_input.Player.SeventhSlot) => 6,
                nameof(_input.Player.EightSlot) => 7,
                nameof(_input.Player.NinthSlot) => 8,
                nameof(_input.Player.TenSlot) => 9,
                _ => throw new ArgumentOutOfRangeException($"Unknown action with name {context.action.name}")
            };

            SelectCell(index);
        }

        private void SelectCell(int index)
        {
            if (index is < 0 or >= 9)
                return;

            _currentSelectedIndex = index;
            var activeCells = GetActiveCells();

            if (index >= activeCells.Count)
                return;

            SelectedCell?.SetSelectionActive(false);
            SelectedCell = activeCells[_currentSelectedIndex];
            SelectedCell.SetSelectionActive(true);

            OnSelectedItemChanged?.Invoke();
        }

        private void CheckInventorySelection()
        {
            if (_playerInventoryView.ActiveCells.Contains(SelectedCell))
                return;

            SelectedCell = null;
            _currentSelectedIndex = -1;
            OnSelectedItemChanged?.Invoke();
        }

        // Didn't use ActiveCells from PlayerInventoryView because they have wrong sorting and can't be sorted because cell positioning set by HorizontalLayoutGroup
        private List<ItemCell> GetActiveCells()
        {
            var result = new List<ItemCell>();

            for (var i = 0; i < _playerInventoryView.CellsContent.childCount; i++)
            {
                var child = _playerInventoryView.CellsContent.GetChild(i).gameObject;
                if (child.activeSelf)
                    result.Add(child.GetComponent<ItemCell>());
            }

            return result;
        }

        private void Bind()
        {
            _playerInventoryView.OnRedrawItemsCells += CheckInventorySelection;

            _input.Player.FirstSlot.performed += OnSlotPerformed;
            _input.Player.SecondSlot.performed += OnSlotPerformed;
            _input.Player.ThirdSlot.performed += OnSlotPerformed;
            _input.Player.FourthSlot.performed += OnSlotPerformed;
            _input.Player.FifthSlot.performed += OnSlotPerformed;
            _input.Player.SixthSlot.performed += OnSlotPerformed;
            _input.Player.SeventhSlot.performed += OnSlotPerformed;
            _input.Player.EightSlot.performed += OnSlotPerformed;
            _input.Player.NinthSlot.performed += OnSlotPerformed;
            _input.Player.TenSlot.performed += OnSlotPerformed;
        }

        private void Expose()
        {
            _playerInventoryView.OnRedrawItemsCells -= CheckInventorySelection;

            _input.Player.FirstSlot.performed -= OnSlotPerformed;
            _input.Player.SecondSlot.performed -= OnSlotPerformed;
            _input.Player.ThirdSlot.performed -= OnSlotPerformed;
            _input.Player.FourthSlot.performed -= OnSlotPerformed;
            _input.Player.FifthSlot.performed -= OnSlotPerformed;
            _input.Player.SixthSlot.performed -= OnSlotPerformed;
            _input.Player.SeventhSlot.performed -= OnSlotPerformed;
            _input.Player.EightSlot.performed -= OnSlotPerformed;
            _input.Player.NinthSlot.performed -= OnSlotPerformed;
            _input.Player.TenSlot.performed -= OnSlotPerformed;
        }
    }
}