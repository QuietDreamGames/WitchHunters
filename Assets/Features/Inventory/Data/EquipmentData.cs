using System;
using System.Collections.Generic;
using Features.Inventory.Item;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using UnityEngine;

namespace Features.Inventory.Data
{
    [Serializable]
    public class EquipmentData
    {
        public List<EquippableItem> items;
        
        public void EquipItem(EquippableItem item)
        {
            
            
            // for (int i = 0; i < items.Count; i++)
            // {
            //     if (((EquippableData)items[i].itemData).equippableType != equippableData.equippableType) continue;
            //     
            //     items[i] = item;
            //     return;
            // }
            //
            // items.Add(item);
        }
        
        public (ModifierType, ModifierData)[] GetAllModifiers()
        {
            var modifiers = new List<(ModifierType, ModifierData)>();

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                for (var j = 0; j < item.itemModifiers.Count; j++)
                {
                    var modifier = item.itemModifiers[j];
                    modifiers.Add((modifier.modifierType, modifier.modifierData));
                }
            }

            return modifiers.ToArray();
        }
    }
}