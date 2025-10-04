using System;
using System.Collections.Generic;
using System.Linq;
using ItemsSystem;
using ItemsSystem.Data;
using UnityEngine;

// ReSharper disable AccessToModifiedClosure

namespace InventorySystem
{
    public class Inventory
    {
        public const int MAX_ITEMS = 10;
        public IReadOnlyCollection<Item> Items => _items;

        private readonly List<Item> _items = new();

        public event Action OnInventoryChanged;

        public bool TryAddItem(Item item)
        {
            var containsThisItem = _items.Any(x => x.Source == item.Source && x.Count < x.Source.MaxCount);

            if (!containsThisItem) // No items with source id in inventory
            {
                if (_items.Count == MAX_ITEMS)
                    return false;
                
                _items.Add(item);

                item.OnItemCountZero += () => OnItemCountZero(item); // Dispose itself
                OnInventoryChanged?.Invoke();
                return true;
            }

            var allMatches = _items.Where(x => x.Source == item.Source).ToList();

            foreach (var itemAdded in allMatches.Select(match => TryAddItemInOther(item, match, false)))
            {
                switch (itemAdded)
                {
                    case true when item.Count == 0:
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                    case false:
                        return false;
                }
            }

            return false;
        }

        public bool TryRemoveItem(Item item, int count = 1) => TryRemoveItem(item.Source, count);

        public bool TryRemoveItem(ItemDataSO source, int count = 1)
        {
            var containsThisItem = _items.Any(x => x.Source == source);

            if (!containsThisItem) // No items with source id in inventory
                return false;

            var allMatches = _items.Where(x => x.Source == source).ToList();

            Debug.Log($"Sum = {allMatches.Sum(x => x.Count)}");
            if (allMatches.Sum(x => x.Count) < count)
                return false;

            foreach (var match in allMatches)
            {
                if (match.Count >= count)
                {
                    Debug.Log(-count);
                    match.TryChangeCount(-count);
                    OnInventoryChanged?.Invoke();
                    return true;
                }

                count -= match.Count;
                Debug.Log(-match.Count);
                match.TryChangeCount(-match.Count);

                if (count != 0)
                    continue;

                OnInventoryChanged?.Invoke();
                return true;
            }

#if UNITY_EDITOR
            Debug.LogWarning($"Can't remove item {source.name}, Count: {count} from Inventory");
#endif
            return false;
        }

        private void OnItemCountZero(Item item)
        {
            _items.Remove(item);
            item.Dispose();
        }

        private static bool TryAddItemInOther(Item item, Item other, bool addFull)
        {
            if (item.Source != other.Source)
                return false;

            switch (addFull)
            {
                case true:
                    if (other.Count >= other.Source.MaxCount || item.Count > other.Source.MaxCount - other.Count)
                        return false;
                    other.TryChangeCount(item.Count);
                    item.TryChangeCount(-item.Count);
                    Debug.Log(item.Count);
                    return true;

                case false:
                    if (other.Count >= other.Source.MaxCount)
                        return false;

                    var need = other.Source.MaxCount - other.Count;

                    if (item.Count <= need)
                    {
                        var add = item.Count;
                        Debug.Log(add);
                        Debug.Log(other.TryChangeCount(add));
                        Debug.Log(item.TryChangeCount(-add));
                    }
                    else
                    {
                        other.TryChangeCount(need);
                        item.TryChangeCount(-need);
                        Debug.Log(need);
                    }

                    return true;
            }
        }
    }
}