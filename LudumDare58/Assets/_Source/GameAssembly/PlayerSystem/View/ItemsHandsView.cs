using System;
using System.Collections.Generic;
using System.Linq;
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

        private void OnSelectedItemChanged()
        {
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