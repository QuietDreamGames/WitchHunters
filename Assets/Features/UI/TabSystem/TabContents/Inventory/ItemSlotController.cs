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

        protected virtual void GetDraggableItem(DraggableItemController dropped)
        {
            var droppedParent = dropped.parentAfterDrag.GetComponent<ItemSlotController>();
            if (!droppedParent.AskForSwap(_draggableItemController))
                return;
            
            droppedParent.OnSwap(_draggableItemController);
            dropped.parentAfterDrag = transform;
            _draggableItemController = dropped;
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
        
        public virtual bool AskForSwap(DraggableItemController swapTo)
        {
            return true;
        }
    }
}