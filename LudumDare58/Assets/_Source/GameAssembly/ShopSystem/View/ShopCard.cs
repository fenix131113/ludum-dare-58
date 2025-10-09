using System;
using ItemsSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopSystem.View
{
    public class ShopCard : MonoBehaviour //TODO: Change trader system
    {
        private const string BUY_LABEL = "Buy";
        private const string UPGRADE_LABEL = "Upgrade";
        
        public event Action<ItemDataSO> OnBuyClicked;
        
        [field: SerializeField] public ItemDataSO TradeItem { get; private set; }

        [SerializeField] private TMP_Text costText;
        [SerializeField] private GameObject boughtTextObject;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text buyButtonText;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnBuyButtonClicked() => OnBuyClicked?.Invoke(TradeItem);
        
        public void SetupCard(int cost, bool isAllLevels, bool isUpgrade)
        {
            costText.text = cost.ToString() + "c.";
            buyButtonText.text = isUpgrade ? UPGRADE_LABEL : BUY_LABEL;
            costText.gameObject.SetActive(!isAllLevels);
            buyButton.gameObject.SetActive(!isAllLevels);
            boughtTextObject.gameObject.SetActive(isAllLevels);
        }

        private void Bind() => buyButton.onClick.AddListener(OnBuyButtonClicked);

        private void Expose() => buyButton.onClick.RemoveAllListeners();
    }
}