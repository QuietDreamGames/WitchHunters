using System;
using System.Collections.Generic;
using Features.Inventory.Item;

namespace Features.Inventory.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<InventoryItem> scrapItems;
        public List<EquippableItem> equipabbleItems;

        public void AddItem(InventoryItem item)
        {
            switch (item)
            {
                case EquippableItem equippableItem:
                    equipabbleItems.Add(equippableItem);
                    return;
                default:
                    scrapItems.Add(item);
                    break;
            }
        }
        
        public void RemoveItem(InventoryItem item)
        {
            switch (item)
            {
                case EquippableItem equippableItem:
                    equipabbleItems.Remove(equippableItem);
                    return;
                default:
                    scrapItems.Remove(item);
                    break;
            }
        }

        public List<InventoryItem> GetAllItems()
        {
            var items = new List<InventoryItem>();
            items.AddRange(equipabbleItems);
            items.AddRange(scrapItems);

            return items;
        }
    }
}