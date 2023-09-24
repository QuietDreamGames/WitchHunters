using Features.Inventory.Data;
using Features.Inventory.Item;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public interface IRemoveItemProcessor
    {
        public void RemoveItem(InventoryItem item);
        public void RemoveItem(ItemData item);
    }
}