using System;
using System.Collections.Generic;
using Features.Character;
using Features.Character.Spawn;
using Features.Inventory;
using Features.Inventory.Data;
using Features.Inventory.Item;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class InventoryTabContent : TabContent
    {
        [SerializeField] private EquipmentSlotController[] _equipmentSlotControllers;
        [SerializeField] private ItemSlotController[] _inventorySlotControllers;
        
        private CombatCharacterController _combatCharacterController;
        
        private ItemSortType _currentSortType;
        private List<InventoryItem> _currentInventoryItems;
        private List<EquippableItem> _currentEquippedItems;
        
        public override void OnSelect()
        {
            base.OnSelect();

            var characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _combatCharacterController = characterHolder.CurrentCharacter;
            
            InitiateInventory();
        }

        private void InitiateInventory()
        {
            _currentSortType = ItemSortType.Armor;
            
            var inventoryController = _combatCharacterController.InventoryController;

            for (int i = 0; i < _equipmentSlotControllers.Length; i++)
            {
                _equipmentSlotControllers[i].Initiate(inventoryController);
            }
            
            RefreshEquippedItems();
            
            RefreshCurrentItemsList();
            
        }

        private void RefreshEquippedItems()
        {
            _currentEquippedItems = new List<EquippableItem>();
            var allItems = _combatCharacterController.InventoryController.InventoryData.equipabbleItems;
            
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].isEquipped)
                    _currentEquippedItems.Add(allItems[i]);
            }
            
            for (int i = 0; i < _equipmentSlotControllers.Length; i++)
            {
                var isSetup = false;
                
                for (int j = 0; j < _currentEquippedItems.Count; j++)
                {
                    if (((EquippableData)_currentEquippedItems[j].itemData).equippableType ==
                        _equipmentSlotControllers[i].EquippableType)
                    {
                        _equipmentSlotControllers[i].SetupItem(_currentEquippedItems[j]);
                        isSetup = true;
                        break;
                    }
                }
                
                if (!isSetup)
                    _equipmentSlotControllers[i].SetupItem(null);
            }
        }

        private void RefreshCurrentItemsList()
        {
            var inventoryController = _combatCharacterController.InventoryController;
            _currentInventoryItems = new List<InventoryItem>();

            switch (_currentSortType)
            {
                case ItemSortType.Armor:
                case ItemSortType.Jewelry:
                    var allItems = inventoryController.InventoryData.equipabbleItems;
                    
                    for (int i = 0; i < allItems.Count; i++)
                    {
                        if (allItems[i].itemData.sortType == _currentSortType && !allItems[i].isEquipped)
                            _currentInventoryItems.Add(allItems[i]);
                    }
                    break;
                case ItemSortType.Scrap:
                    _currentInventoryItems = inventoryController.InventoryData.stackableItems;
                    break;

                case ItemSortType.Quest:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < _inventorySlotControllers.Length; i++)
            {
                if (i >= _currentInventoryItems.Count)
                {
                    _inventorySlotControllers[i].SetupItem(null);
                    continue;
                }
                
                _inventorySlotControllers[i].SetupItem(_currentInventoryItems[i]);
            }
        }
        
        public void OnSortTypeChanged(ItemSortType sortType)
        {
            _currentSortType = sortType;
            RefreshCurrentItemsList();
        }
        
        public void OnEquipItem(EquippableItem item)
        {
            _combatCharacterController.InventoryController.EquipItem(item);
            RefreshEquippedItems();
            RefreshCurrentItemsList();
        }
    }
}