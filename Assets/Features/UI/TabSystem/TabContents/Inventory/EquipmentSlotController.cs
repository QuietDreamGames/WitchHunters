using Features.Inventory;
using Features.Inventory.Data;
using Features.Inventory.Item;
using UnityEngine;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class EquipmentSlotController : ItemSlotController
    {
        [SerializeField] private EquippableType _equippableType;
        public EquippableType EquippableType => _equippableType;
        
        private InventoryTabContent _inventoryTabContent;

        public void Initiate(InventoryTabContent inventoryTabContent)
        {
            _inventoryTabContent = inventoryTabContent;
        }

        protected override void GetDraggableItem(DraggableItemController dropped) //triggered when something is dragged in this slot
        {
            if (dropped == _draggableItemController) //same item
                return;
            
            var droppedParent = dropped.parentAfterDrag.GetComponent<ItemSlotController>();
            if (!droppedParent.AskForSwap(_draggableItemController)) //previous item slot doesnt want to swap
                return;
            
            if (!AskForSwap(dropped)) //this slot doesnt want to swap
                return;

            if (_draggableItemController.CurrentItem != null)
                _inventoryTabContent.OnUnequipItem((EquippableItem)_draggableItemController.CurrentItem);
            
            droppedParent.OnSwap(_draggableItemController);
            dropped.parentAfterDrag = transform;
            _draggableItemController = dropped;
            
            _inventoryTabContent.OnEquipItem((EquippableItem)_draggableItemController.CurrentItem);
        }

        public override void OnSwap(DraggableItemController swapTo) //triggered when from this slot something is dragged out
        {
            if (swapTo == _draggableItemController) //same item
                return;
            
            if (_draggableItemController.CurrentItem != null)
                _inventoryTabContent.OnUnequipItem((EquippableItem)_draggableItemController.CurrentItem);
            
            swapTo.transform.SetParent(transform);
            swapTo.transform.localPosition = Vector3.zero;
            _draggableItemController = swapTo;
            
            if (_draggableItemController.CurrentItem != null)
                _inventoryTabContent.OnEquipItem((EquippableItem)_draggableItemController.CurrentItem);
        }

        public override bool AskForSwap(DraggableItemController swapTo)
        {
            if (swapTo.CurrentItem == null)
                return true;
            
            if (swapTo.CurrentItem is not EquippableItem equipItemToSwap)
                return false;
            
            if (((EquippableData)equipItemToSwap.itemData).equippableType != _equippableType)
                return false;
            
            return base.AskForSwap(swapTo);
        }
    }
}