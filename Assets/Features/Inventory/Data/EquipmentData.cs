using System;
using System.Collections.Generic;
using Features.Inventory.Item;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;

namespace Features.Inventory.Data
{
    [Serializable]
    public class EquipmentData
    {
        public EquippableItem helmet;
        public EquippableItem chest;
        public EquippableItem arms;
        public EquippableItem legs;
        
        public EquippableItem amulet;
        public EquippableItem ring;
        
        public void EquipItem(EquippableItem item)
        {
            switch (item.equippableType)
            {
                case EquippableType.Helmet:
                    helmet = item;
                    break;
                case EquippableType.Chest:
                    chest = item;
                    break;
                case EquippableType.Arms:
                    arms = item;
                    break;
                case EquippableType.Legs:
                    legs = item;
                    break;
                case EquippableType.Amulet:
                    amulet = item;
                    break;
                case EquippableType.Ring:
                    ring = item;
                    break;
            }
        }
        
        public (ModifierType, ModifierData)[] GetAllModifiers()
        {
            var modifiers = new List<(ModifierType, ModifierData)>();
            
            if (helmet.itemData != null)
            {
                for (var i = 0; i < helmet.itemModifiers.Count; i++)
                {
                    var itemModifier = helmet.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }
            
            if (chest.itemData != null)
            {
                for (var i = 0; i < chest.itemModifiers.Count; i++)
                {
                    var itemModifier = chest.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }
            
            if (arms.itemData != null)
            {
                for (var i = 0; i < arms.itemModifiers.Count; i++)
                {
                    var itemModifier = arms.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }
            
            if (legs.itemData != null)
            {
                for (var i = 0; i < legs.itemModifiers.Count; i++)
                {
                    var itemModifier = legs.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }
            
            if (amulet.itemData != null)
            {
                for (var i = 0; i < amulet.itemModifiers.Count; i++)
                {
                    var itemModifier = amulet.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }
            
            if (ring.itemData != null)
            {
                for (var i = 0; i < ring.itemModifiers.Count; i++)
                {
                    var itemModifier = ring.itemModifiers[i];
                    modifiers.Add((itemModifier.modifierType, itemModifier.modifierData));
                }
            }

            return modifiers.ToArray();
        }
    }
}