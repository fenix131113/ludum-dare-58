using System;
using System.Collections.Generic;
using InteractionSystem;
using ItemsSystem.Data;
using UnityEngine;

namespace ShopSystem
{
    public class Trader : MonoBehaviour, IInteractable
    {
        public IReadOnlyList<TradeData> Trades => trades;
        
        [SerializeField] private List<TradeData> trades;
        
        public event Action OnInteracted;

        public void TryBuy(TradeData trade)
        {
            
        }
        
        public void Interact()
        {
            OnInteracted?.Invoke();
        }

        [Serializable]
        public class TradeData
        {
            [field: SerializeField] public ItemDataSO Item { get; private set; }
            [field: SerializeField] public int Cost { get; private set; }
        }
    }
}