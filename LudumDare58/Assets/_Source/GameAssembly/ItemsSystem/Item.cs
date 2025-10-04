using System;
using ItemsSystem.Data;
using UnityEngine;

namespace ItemsSystem
{
    public class Item : IDisposable
    {
        public int Count;
        
        public readonly ItemDataSO Source;

        /// <summary>
        /// Dispose itself
        /// </summary>
        public event Action OnItemChanged;
        public event Action OnItemCountZero;

        public Item(ItemDataSO source, int count)
        {
            Source = source;
            Count = Mathf.Clamp(count, 1, source.MaxCount);
        }
        
        ~Item() => Dispose();

        public bool TryChangeCount(int value)
        {
            //Debug.Log($"Trying to change count of {Source.ItemName} to {Count + value}");
            if (Count + value > Source.MaxCount || Count + value < 0)
                return false;
            
            Count = Mathf.Clamp(Count + value, 0, Source.MaxCount);
            OnItemChanged?.Invoke();
            
            if(Count == 0)
                OnItemCountZero?.Invoke();
            
            return true;
        }

        public Item Clone(int newCount = -1) => new(Source, newCount == -1 ? Count : newCount);

        public void Dispose()
        {
            OnItemChanged = null;
            OnItemCountZero = null;
        }
    }
}