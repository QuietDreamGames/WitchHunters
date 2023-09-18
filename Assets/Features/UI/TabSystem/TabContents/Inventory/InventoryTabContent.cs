using System;
using System.Collections;
using System.Collections.Generic;
using Features.Character;
using Features.Character.Spawn;
using Features.Inventory;
using Features.Inventory.Data;
using Features.Inventory.Item;
using Features.ServiceLocators.Core;
using TMPro;
using UnityEngine;

namespace Features.UI.TabSystem.TabContents.Inventory
{
    public class InventoryTabContent : TabContent
    {
        [SerializeField] private EquipmentSlotController[] _equipmentSlotControllers;
        [SerializeField] private ItemSlotController[] _inventorySlotControllers;
        
        [SerializeField] private InventoryTabButton[] _tabButtons;
        
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private TextMeshProUGUI _weightText;
        
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
            
            for (int i = 0; i < _equipmentSlotControllers.Length; i++)
            {
                _equipmentSlotControllers[i].Initiate(this);
            }

            for (int i = 0; i < _tabButtons.Length; i++)
            {
                _tabButtons[i].Initiate(this);
            }
            
            _tabButtons[0].OnSelect();
            
            RefreshEquippedItems();
            
            RefreshCurrentItemsList();

            RefreshResources();
        }
        
        private void RefreshResources()
        {
            _currencyText.text = _combatCharacterController.InventoryController.InventoryData.currency.ToString();
            _weightText.text = $"{_combatCharacterController.InventoryController.Weight}/" +
                               $"{_combatCharacterController.InventoryController.MaxWeight}";
            _weightText.color = _combatCharacterController.InventoryController.IsOverweight ? Color.red : Color.white;
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
        
        public void OnSortTypeChanged(InventoryTabButton tabButton)
        {
            _currentSortType = tabButton.TabType;

            for (int i = 0; i < _tabButtons.Length; i++)
            {
                if (_tabButtons[i] == tabButton)
                {
                    _tabButtons[i].OnSelect();
                }
                else
                {
                    _tabButtons[i].OnDeselect();
                }
            }
            
            RefreshCurrentItemsList();
        }
        
        public void OnEquipItem(EquippableItem item)
        {
            _combatCharacterController.InventoryController.EquipItem(item);
            
            // StartCoroutine(OnRefreshInventory());
        }
        
        public void OnUnequipItem(EquippableItem item)
        {
            _combatCharacterController.InventoryController.UnequipItem(item);

            if (_currentSortType == item.itemData.sortType) return;
            
            _currentSortType = item.itemData.sortType;
            StartCoroutine(OnRefreshInventory());
        }
        
        private IEnumerator OnRefreshInventory()
        {
            yield return new WaitForEndOfFrame();
            
            RefreshEquippedItems();
            RefreshCurrentItemsList();
        } 
    }
}