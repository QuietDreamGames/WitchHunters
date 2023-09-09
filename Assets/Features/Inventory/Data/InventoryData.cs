using System;
using System.Collections.Generic;
using Features.Inventory.Item;
using UnityEngine.Serialization;

namespace Features.Inventory.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<InventoryItem> stackableItems;
        public List<EquippableItem> equipabbleItems;

        public void AddItem(ItemData itemData)
        {
            switch (itemData)
            {
                case EquippableData equippableData:
                    var equippableItem = new EquippableItem
                    {
                        itemData = equippableData,
                        isEquipped = false,
                        upgradeLevel = 0
                    };
                    
                    equippableItem.ResetModifiers();
                    
                    equipabbleItems.Add(equippableItem);
                    return;
                default:
                    for (var i = 0; i < stackableItems.Count; i++)
                    {
                        if (stackableItems[i].itemData == itemData)
                        {
                            stackableItems[i].amount++;
                            return;
                        }
                    }
                    var item = new InventoryItem
                    {
                        itemData = itemData,
                        amount = 1
                    };
                    
                    stackableItems.Add(item);
                    break;
            }
        }

        public void AddItem(InventoryItem inventoryItem)
        {
            switch (inventoryItem)
            {
                case EquippableItem equippableItem:
                    equipabbleItems.Add(equippableItem);
                    return;
                default:
                    AddItem(inventoryItem.itemData);
                    return;
            }
        }
        
        public void RemoveItem(ItemData itemData)
        {
            switch (itemData)
            {
                case EquippableData equippableData:
                    for (var i = 0; i < equipabbleItems.Count; i++)
                    {
                        if (equipabbleItems[i].itemData == equippableData)
                        {
                            equipabbleItems.RemoveAt(i);
                            return;
                        }
                    }
                    break;
                default:
                    for (var i = 0; i < stackableItems.Count; i++)
                    {
                        if (stackableItems[i].itemData == itemData)
                        {
                            stackableItems[i].amount--;
                            if (stackableItems[i].amount <= 0)
                                stackableItems.RemoveAt(i);
                            return;
                        }
                    }
                    break;
            }
        }

        public void RemoveItem(InventoryItem inventoryItem)
        {
            switch (inventoryItem)
            {
                case EquippableItem equippableItem:
                    for (var i = 0; i < equipabbleItems.Count; i++)
                    {
                        if (equipabbleItems[i] == equippableItem)
                        {
                            equipabbleItems.RemoveAt(i);
                            return;
                        }
                    }
                    break;
                default:
                    RemoveItem(inventoryItem.itemData);
                    break;
            }
        }
        
        public List<InventoryItem> GetAllItems()
        {
            var items = new List<InventoryItem>();
            items.AddRange(equipabbleItems);
            items.AddRange(stackableItems);

            return items;
        }
    }
}