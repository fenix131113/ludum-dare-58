using System;
using System.Collections.Generic;
using System.Linq;
using InteractionSystem;
using InventorySystem;
using ItemsSystem.Data;
using LevelsSystem;
using PlayerSystem;
using UnityEngine;
using VContainer;

namespace ShopSystem
{
    public class Trader : MonoBehaviour, IInteractable //TODO: Make upgrade abstract system, maybe through IUpgradable or AUpgradeHandItem, extend max upgrades level
    {
        public IReadOnlyDictionary<ItemDataSO, int> BoughtItems => _boughtItems;
        public IReadOnlyList<TradeData> Trades => trades;

        [SerializeField] private ItemDataSO firstItemData; //TODO: Change native implementation
        [SerializeField] private List<TradeData> trades;

        [Inject] private PlayerResources _playerResources;
        [Inject] private Inventory _inventory;
        [Inject] private UpgradesImplementer _upgradesImplementer;
        private Dictionary<ItemDataSO, int> _boughtItems = new();

        private InterLevelData _interLevelData;

        public event Action OnInteracted;

        private void Awake()
        {
            _interLevelData = FindFirstObjectByType<InterLevelData>();
            LoadBoughtItems();
        }

        private void LoadBoughtItems()
        {
            _boughtItems = _interLevelData.BoughtItems.ToDictionary(x => x.Key, x => x.Value);

            _boughtItems.TryAdd(firstItemData, 0);
        }

        private void CheckForBoughtItems()
        {
            foreach (var item in _boughtItems.Where(item => item.Value > 0))
                _upgradesImplementer.ImplementUpgrade(item.Key);
        }

        public bool TryBuy(ItemDataSO item)
        {
            var trade = trades.First(x => x.Item == item);

            if (_boughtItems.ContainsKey(item) && _boughtItems[item] + 1 >= trade.TradeDataGroups.Count)
                return false;

            if (_boughtItems.TryGetValue(item, out var level) && level == 0)
            {
                if (!_playerResources.TryChangeCoins(-trade.TradeDataGroups[level].Cost))
                    return false;

                _boughtItems[item]++;
                CheckForBoughtItems();
            }
            else
            {
                if (!_playerResources.TryChangeCoins(-trade.TradeDataGroups[level].Cost))
                    return false;

                if (!_inventory.IsItemInInventory(item))
                    _inventory.TryAddItem(item.GenerateItemInstance());
                _boughtItems[item] = 0;
            }

            _interLevelData.SetBoughtItems(_boughtItems);
            return true;
        }

        public void Interact()
        {
            OnInteracted?.Invoke();
        }

        [Serializable]
        public class TradeData
        {
            [field: SerializeField] public ItemDataSO Item { get; private set; }
            [field: SerializeField] public List<TradeDataGroup> TradeDataGroups { get; private set; }
        }

        [Serializable]
        public class TradeDataGroup
        {
            [field: SerializeField] public int Cost { get; private set; }
        }
    }
}