using Features.Character.Spawn;
using Features.Inventory;
using Features.Inventory.Item;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Test
{
    public class TestInventory : MonoBehaviour
    {
        [SerializeField] private EquippableItem _armorItem;
        
        public void AddCharredItem()
        {
            var character = ServiceLocator.Resolve<CharacterHolder>().CurrentCharacter;
            var inventory = character.InventoryController;

            // _armorItem.SetModifier(ModifierType.MoveSpeed,
            //     new ModifierData(0.1f, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            
            inventory.AddItem(_armorItem);
            
        }
    }
}