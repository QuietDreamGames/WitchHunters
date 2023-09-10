using System;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;

namespace Features.Inventory.Data
{
    [Serializable]
    public class ItemModifier
    {
        public ModifierType modifierType;
        public ModifierData modifierData;
        
        public ItemModifier CopyModifiers()
        {
            return new ItemModifier
            {
                modifierType = modifierType,
                modifierData = modifierData
            };
        }
    }
}