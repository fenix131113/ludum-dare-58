using ItemsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem.View
{
    public class ItemCell : MonoBehaviour
    {
        public Item CurrentItem { get; private set; }

        [SerializeField] private Image iconImg;

        public void SetupCell(Item item)
        {
            CurrentItem = item;
            DrawCell();
        }

        public void ClearCell() => CurrentItem = null;

        private void DrawCell() => iconImg.sprite = CurrentItem.Source.Icon;
    }
}