using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.UI.TabSystem
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private TabType _tabType;
        [SerializeField] private Image _background;
        
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _hoverColor;
        
        public TabType TabType => _tabType;
        
        private TabGroup _tabGroup;
        
        private bool _isSelected;
        
        public void Initiate(TabGroup tabGroup)
        {
            _tabGroup = tabGroup;    
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            _background.color = _hoverColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSelected) return;
            _tabGroup.OnTabSelectedWithClick(this);
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