using Features.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class InventoryTabButton2 : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private ItemSortType _tabType;
        [SerializeField] private TextMeshProUGUI _text;

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
            _text.color = _hoverColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSelected) return;
            // _tabContent.OnSortTypeChanged(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            _text.color = _normalColor;
        }

        public void OnSelect()
        {
            _isSelected = true;
            _text.color = _selectedColor;
        }

        public void OnDeselect()
        {
            _isSelected = false;
            _text.color = _normalColor;
        }
    }
}