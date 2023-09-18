using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class DropAreaController : MonoBehaviour, IDropHandler
    {
        protected DraggableItemController draggableItemController;
        
        public void OnDrop(PointerEventData eventData)
        {
            draggableItemController = eventData.pointerDrag.GetComponent<DraggableItemController>();

            ProcessDrop();
        }
        
        protected virtual void ProcessDrop()
        {
            
        }
    }
}