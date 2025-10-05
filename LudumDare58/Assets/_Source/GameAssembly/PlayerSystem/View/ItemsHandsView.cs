using System;
using System.Collections.Generic;
using System.Linq;
using ItemsSystem;
using ItemsSystem.Data;
using UnityEngine;
using VContainer;

namespace PlayerSystem.View
{
    public class ItemsHandsView : MonoBehaviour
    {
        [SerializeField] private List<ItemViewGroup> itemGroups;

        [Inject] private ItemSelector _itemSelector;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnSelectedItemChanged(Item oldItem, Item newItem)
        {
            if (_itemSelector.SelectedCell && oldItem != newItem && newItem != null)
            {
                if(oldItem is { Source: WeaponItemDataSO })
                    itemGroups.First(x => x.Item == oldItem.Source).HandsItem.Deactivate();
            }
            
            if (!_itemSelector.SelectedCell || _itemSelector.SelectedCell.CurrentItem.Source is not WeaponItemDataSO)
            {
                ClearHands();
                return;
            }
            
            ActivateSelectedItem();
        }

        private void ClearHands() => itemGroups.ForEach(x => x.HandsItem.Deactivate());

        private void ActivateSelectedItem()
        {
            var selectedItem = _itemSelector.SelectedCell.CurrentItem.Source as WeaponItemDataSO;
            
            itemGroups.First(x => x.Item == selectedItem).HandsItem.Activate();
        }

        private void Bind() => _itemSelector.OnSelectedItemChanged += OnSelectedItemChanged;

        private void Expose() => _itemSelector.OnSelectedItemChanged -= OnSelectedItemChanged;

        [Serializable]
        public class ItemViewGroup
        {
            [field: SerializeField] public ItemDataSO Item { get; private set; }
            [field: SerializeField] public APlayerHandItem HandsItem { get; private set; }
        }
    }
}