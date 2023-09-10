using System;
using System.Collections.Generic;
using Features.Inventory.Data;

namespace Features.Inventory.Item
{
    [Serializable]
    public class EquippableItem : InventoryItem
    {
        public List<ItemModifier> itemModifiers;
        public bool isEquipped;
        
        public int upgradeLevel;

        public void ResetModifiers()
        {
            itemModifiers = new List<ItemModifier>();
            var equippableData = (EquippableData) itemData;
            
            for (var i = 0; i < equippableData.baseItemModifiers.Length; i++)
            {
                var baseItemModifier = equippableData.baseItemModifiers[i];
                itemModifiers.Add(baseItemModifier.CopyModifiers());
            }
        }
    }
}