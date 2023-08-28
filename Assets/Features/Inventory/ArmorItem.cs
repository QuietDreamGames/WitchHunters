using System;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;

namespace Features.Inventory
{
    [Serializable]
    public class ArmorItem : InventoryItem
    {
        public int tier;
        public ArmorType armorType;
        public int baseArmor;
        
        public bool hasModifier;
        
        private ModifierData _modifierData;
        private ModifierType _modifierType;

        public (ModifierType, ModifierData) GetModifier()
        {
            return (_modifierType, _modifierData);
        }
        
        public void SetModifier(ModifierType type, ModifierData data)
        {
            _modifierType = type;
            _modifierData = data;
            
            hasModifier = _modifierData != null;
        }
    }
}