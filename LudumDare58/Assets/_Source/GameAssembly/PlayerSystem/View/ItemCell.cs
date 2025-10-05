using ItemsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSystem.View
{
    public class ItemCell : MonoBehaviour
    {
        public Item CurrentItem { get; private set; }

        [SerializeField] private Image iconImg;
        [SerializeField] private GameObject selection;

        public void SetupCell(Item item)
        {
            CurrentItem = item;
            DrawCell();
        }

        public void ClearCell()
        {
            CurrentItem = null;
            SetSelectionActive(false);
        }

        private void DrawCell() => iconImg.sprite = CurrentItem.Source.Icon;

        public void SetSelectionActive(bool active) => selection.SetActive(active);
    }
}