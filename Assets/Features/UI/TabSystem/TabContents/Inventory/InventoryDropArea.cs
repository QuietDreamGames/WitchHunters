using Features.Inventory.Item;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class InventoryDropArea : DropAreaController
    {
        protected override void ProcessSingleDrop()
        {
            if (draggableItemController == null) return;
            
            var item = draggableItemController.CurrentItem;
                
            if (item is EquippableItem equippableItem)
            {
                if (equippableItem.isEquipped) return;
                removeItemProcessor.RemoveItem(equippableItem);
            }
            
            removeItemProcessor.RemoveItem(draggableItemController.CurrentItem);
        }
    }
}