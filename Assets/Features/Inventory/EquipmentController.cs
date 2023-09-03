using System.Collections.Generic;
using Features.Character;
using Features.Inventory.Data;
using Features.Inventory.Item;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Inventory
{
    public class EquipmentController : MonoBehaviour
    {
        private ModifiersContainer _modifiersContainer;
        
        private List<EquippableItem> _equippedItems;
        
        private List<(ModifierType, ModifierData)> _modifiersData;
        
        public void Initiate(InventoryController inventoryController, ModifiersContainer modifiersContainer)
        {
            _modifiersContainer = modifiersContainer;
            
            inventoryController.OnEquipItem -= OnEquipItem;
            inventoryController.OnEquipItem += OnEquipItem;
            
            inventoryController.OnUnequipItem -= OnUnequipItem;
            inventoryController.OnUnequipItem += OnUnequipItem;
            
            var allItems = inventoryController.InventoryData.equipabbleItems;
            _equippedItems = new List<EquippableItem>();
            _modifiersData = new List<(ModifierType, ModifierData)>();

            
            for (var i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].isEquipped)
                    OnEquipItem(allItems[i]);
            }

            RecalculateModifiers();
        }
        
        
        
        private void OnEquipItem(EquippableItem item)
        {
            #region Checks

            if (item.itemData == null)
            {
                Debug.LogError($"Bad item data: {item}");
                return;
            }
            
            var equippableData = (EquippableData) item.itemData;
            
            if (equippableData == null)
            {
                Debug.LogError($"Bad item equippable data: {item.itemData}");
                return;
            }

            #endregion

            var isThisTypeAlreadyEquipped = false;
            var itemToUnequip = (EquippableItem) null;
            
            
            for (int i = 0; i < _equippedItems.Count; i++)
            {
                if (((EquippableData)_equippedItems[i].itemData).equippableType != equippableData.equippableType) continue;
                isThisTypeAlreadyEquipped = true;
                itemToUnequip = _equippedItems[i];
            }
            
            if (isThisTypeAlreadyEquipped)
            {
                OnUnequipItem(itemToUnequip);
            }
            
            _equippedItems.Add(item);
            
            RecalculateModifiers();
        }
        
        private void OnUnequipItem(EquippableItem item)
        {
            #region Checks

            if (item.itemData == null)
            {
                Debug.LogError($"Bad item data: {item}");
                return;
            }
            
            var equippableData = (EquippableData) item.itemData;
            
            if (equippableData == null)
            {
                Debug.LogError($"Bad item equippable data: {item.itemData}");
                return;
            }

            #endregion
            
            _equippedItems.Remove(item);
            
            RecalculateModifiers();
        }

        private void RecalculateModifiers()
        {
            foreach ((ModifierType, ModifierData) entry in _modifiersData)
            {
                _modifiersContainer.Remove(entry.Item1, entry.Item2);
            }
            
            _modifiersData.Clear();
            
            for (var i = 0; i < _equippedItems.Count; i++)
            {
                var item = _equippedItems[i];
                for (var j = 0; j < item.itemModifiers.Count; j++)
                {
                    var modifier = item.itemModifiers[j];
                    _modifiersData.Add((modifier.modifierType, modifier.modifierData));
                    _modifiersContainer.Add(modifier.modifierType, modifier.modifierData);
                }
            }
        }
    }
}