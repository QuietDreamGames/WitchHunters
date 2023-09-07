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
        
        private InventoryController _inventoryController;

        public void Initiate(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        protected override void GetDraggableItem(DraggableItemController draggableItemController)
        {
            var item = draggableItemController.CurrentItem;
            
            if (item is not EquippableItem equippableItem)
                return;

            item = equippableItem;
            var itemData = (EquippableData) item.itemData;
            if (itemData.equippableType != _equippableType)
                return;
            
            _inventoryController.UnequipItem((EquippableItem)_draggableItemController.CurrentItem);
            _inventoryController.EquipItem((EquippableItem)draggableItemController.CurrentItem);
            base.GetDraggableItem(draggableItemController);
        }

        public override void OnSwap(DraggableItemController swapTo)
        {
            _inventoryController.UnequipItem((EquippableItem)_draggableItemController.CurrentItem);
            _inventoryController.EquipItem((EquippableItem)swapTo.CurrentItem);
            base.OnSwap(swapTo);
        }
    }
}