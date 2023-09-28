using Features.Inventory.Item;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class DraggableItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas _canvas;
        [HideInInspector] public Transform parentAfterDrag;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amountText;
        
        private InventoryItem _currentItem;
        private bool _isDraggable;
        
        public InventoryItem CurrentItem => _currentItem;
        
        public void SetupItem(InventoryItem item)
        {
            _currentItem = item;
            
            if (item == null)
            {
                _icon.sprite = null;
                _isDraggable = false;
                _icon.gameObject.SetActive(false);
                return;
            }
            
            _icon.sprite = _currentItem.itemData.icon;
            _isDraggable = true;
            _icon.gameObject.SetActive(true);
            
            _amountText.text = _currentItem.amount > 1 ? _currentItem.amount.ToString() : "";
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            parentAfterDrag = transform.parent;
            transform.SetParent(_canvas.transform);
            _icon.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
            _icon.raycastTarget = true;
        }
    }
}