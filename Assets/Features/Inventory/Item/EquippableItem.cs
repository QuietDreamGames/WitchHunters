using System;
using System.Collections.Generic;
using Features.Inventory.Data;

namespace Features.Inventory.Item
{
    [Serializable]
    public class EquippableItem : InventoryItem
    {
        public EquippableType equippableType;
        
        public List<ItemModifier> itemModifiers;
        
        public int upgradeLevel;
        
        public EquippableItem()
        {
            ResetModifiers();
        }

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