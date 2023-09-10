using System;
using Features.Inventory.Data;

namespace Features.Inventory.Item
{
    [Serializable]
    public class InventoryItem
    {
        public ItemData itemData;
        public int amount;
    }
}