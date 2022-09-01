using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.Services
{
    public class ClickableText : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Color _downColor;
        
        public Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = _selectedColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = _unselectedColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _text.color = _downColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _text.color = _selectedColor;
        }
    }
}