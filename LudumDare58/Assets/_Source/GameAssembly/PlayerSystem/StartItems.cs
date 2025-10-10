using System.Collections.Generic;
using InventorySystem;
using ItemsSystem.Data;
using UnityEngine;
using VContainer;

namespace PlayerSystem
{
    public class StartItems : MonoBehaviour
    {
        [SerializeField] private List<ItemDataSO> startItems;

        [Inject] private Inventory _inventory;

        private void Start()
        {
            startItems.ForEach(x =>
            {
                if (!_inventory.TryAddItem(x.GenerateItemInstance()))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Failed to add Item {x.ItemName}");
#endif
                }
            });
        }
    }
}