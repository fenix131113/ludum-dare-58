using ItemsSystem.Data;
using PlayerSystem;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ShopSystem.View
{
    public class TraderView : MonoBehaviour
    {
        [SerializeField] private Trader trader;
        [SerializeField] private GameObject canvas;
        [SerializeField] private Button closeButton;
        [SerializeField] private ShopCard[] shopCards;

        [Inject] private InputSystem_Actions _input;

        private void Start()
        {
            Bind();
            UpdateCards();
        }

        private void OnDestroy() => Expose();

        private void UpdateCards()
        {
            for (var i = 0; i < trader.Trades.Count; i++)
            {
                var trade = trader.Trades[i];

                 if (trader.BoughtItems.TryGetValue(trade.Item, out var itemLevel))
                 {
                     if(trade.TradeDataGroups.Count > itemLevel + 1)
                        shopCards[i].SetupCard(trade.TradeDataGroups[itemLevel + 1].Cost,
                         itemLevel + 1 >= trade.TradeDataGroups.Count, itemLevel + 1 > 0);
                     else
                         shopCards[i].SetupCard(0, true, true);
                 }
                 else
                    shopCards[i].SetupCard(trade.TradeDataGroups[0].Cost, false, false);
            }
        }

        private void OnTryingBuy(ItemDataSO item)
        {
            if (trader.TryBuy(item))
                UpdateCards();
        }

        private void OpenShop()
        {
            _input.Player.Disable();
            canvas.gameObject.SetActive(true);
        }

        private void CloseShop()
        {
            _input.Player.Enable();
            canvas.gameObject.SetActive(false);
        }

        private void Bind()
        {
            trader.OnInteracted += OpenShop;
            closeButton.onClick.AddListener(CloseShop);

            foreach (var shopCard in shopCards)
                shopCard.OnBuyClicked += OnTryingBuy;
        }

        private void Expose()
        {
            trader.OnInteracted += OpenShop;
            closeButton.onClick.RemoveAllListeners();

            foreach (var shopCard in shopCards)
                shopCard.OnBuyClicked -= OnTryingBuy;
        }
    }
}