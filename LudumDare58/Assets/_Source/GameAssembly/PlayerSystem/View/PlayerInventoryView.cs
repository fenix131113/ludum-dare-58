using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace PlayerSystem.View
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField] private ItemCell inventoryCellPrefab;
        [SerializeField] private Transform content;

        [Inject] private Inventory _inventory;
        private ItemCell _currentBindingCell;

        private readonly List<ItemCell> _activeCells = new();
        private readonly List<ItemCell> _cellsPool = new();

        private void Start() => Bind();

        private void Update()
        {
            if(Mouse.current.rightButton.isPressed)
                Debug.Log(string.Join("\n", _inventory.Items.Select(x => x.Source.ItemName + " - " + x.Count)));
        }

        private void OnDestroy() => Expose();

        private void DrawInventory()
        {
            _activeCells.ToList().ForEach(AddCellToPool);
            
            foreach (var item in _inventory.Items)
            {
                var cell = TakeCellFromPool();

                if (!cell)
                    cell = SpawnNewCell();

                _activeCells.Add(cell);
                cell.SetupCell(item);
                cell.gameObject.SetActive(true);
                _currentBindingCell = cell;
                item.OnItemCountZero += OnItemCountZero; // OnItemCountZero dispose itself
            }
        }

        private void OnItemCountZero() => AddCellToPool(_currentBindingCell);

        private ItemCell SpawnNewCell() => Instantiate(inventoryCellPrefab, content);

        private ItemCell TakeCellFromPool()
        {
            var took = _cellsPool.Count == 0 ? null : _cellsPool[^1];
            
            if(took)
                _cellsPool.RemoveAt(_cellsPool.Count - 1);
            
            return took;
        }

        private void AddCellToPool(ItemCell cell)
        {
            if (_cellsPool.Contains(cell))
                return;

            if (_activeCells.Contains(cell))
                _activeCells.Remove(cell);
            
            _cellsPool.Add(cell);
            cell.CurrentItem.OnItemCountZero -= OnItemCountZero;
            cell.ClearCell();
            cell.gameObject.SetActive(false);
        }

        private void Bind() => _inventory.OnInventoryChanged += DrawInventory;

        private void Expose() => _inventory.OnInventoryChanged -= DrawInventory;
    }
}