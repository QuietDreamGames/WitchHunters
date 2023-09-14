using Features.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class InventoryTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private ItemSortType _tabType;
        [SerializeField] private Image _background;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _hoverColor;

        public ItemSortType TabType => _tabType;

        private InventoryTabContent _tabContent;

        private bool _isSelected;

        public void Initiate(InventoryTabContent tabContent)
        {
            _tabContent = tabContent;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            _background.color = _hoverColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSelected) return;
            _tabContent.OnSortTypeChanged(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            _background.color = _normalColor;
        }

        public void OnSelect()
        {
            _isSelected = true;
            _background.color = _selectedColor;
        }

        public void OnDeselect()
        {
            _isSelected = false;
            _background.color = _normalColor;
        }
    }
}