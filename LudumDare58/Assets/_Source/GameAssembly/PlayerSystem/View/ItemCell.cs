using ItemsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using VContainer;

namespace PlayerSystem.View
{
    public class ItemCell : MonoBehaviour, IPointerClickHandler
    {
        public Item CurrentItem { get; private set; }

        [SerializeField] private Image iconImg;
        [SerializeField] private GameObject selection;

        [Inject] private ItemSelector _itemSelector;

        public void SetupCell(Item item)
        {
            ObjectInjector.InjectGameObject(gameObject);
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

        public void OnPointerClick(PointerEventData eventData) => _itemSelector.SelectCell(this);
    }
}