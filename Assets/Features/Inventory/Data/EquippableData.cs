using UnityEngine;

namespace Features.Inventory.Data
{
    [CreateAssetMenu(fileName = "EquippableData", menuName = "Inventory/EquippableData", order = 0)]
    public class EquippableData : ItemData
    {
        public EquippableType equippableType;
        public ItemModifier[] baseItemModifiers;
    }
}