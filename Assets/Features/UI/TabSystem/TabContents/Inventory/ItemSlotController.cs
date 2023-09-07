using Features.Inventory.Item;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class ItemSlotController : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected DraggableItemController _draggableItemController;
        
        public void SetupItem(InventoryItem item)
        {
            _draggableItemController.SetupItem(item);
        }

        protected virtual void GetDraggableItem(DraggableItemController draggableItemController)
        {
            draggableItemController.parentAfterDrag.GetComponent<ItemSlotController>()
                .OnSwap(_draggableItemController);
            draggableItemController.parentAfterDrag = transform;
            _draggableItemController = draggableItemController;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var dropped = eventData.pointerDrag.GetComponent<DraggableItemController>();
            GetDraggableItem(dropped);
        }
        
        public virtual void OnSwap(DraggableItemController swapTo)
        {
            swapTo.transform.SetParent(transform);
            swapTo.transform.localPosition = Vector3.zero;
            _draggableItemController = swapTo;
        }
    }
}