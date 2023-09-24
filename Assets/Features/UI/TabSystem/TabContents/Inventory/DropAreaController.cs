using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public abstract class DropAreaController : MonoBehaviour, IDropHandler
    {
        protected DraggableItemController draggableItemController;
        protected IRemoveItemProcessor removeItemProcessor;
        
        public void Initiate(IRemoveItemProcessor removeItemProcessor)
        {
            draggableItemController = null;
            this.removeItemProcessor = removeItemProcessor;
        }
        
        
        public void OnDrop(PointerEventData eventData)
        {
            draggableItemController = eventData.pointerDrag.GetComponent<DraggableItemController>();

            ProcessSingleDrop();
        }

        protected abstract void ProcessSingleDrop();
    }
}