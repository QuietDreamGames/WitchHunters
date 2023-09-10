using System;
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
    public class InventoryController : MonoBehaviour, ISavable
    {
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;
        [SerializeField] private InventoryData _inventoryData;
        
        public Action<EquippableItem> OnEquipItem;
        public Action<EquippableItem> OnUnequipItem;
        
        public InventoryData InventoryData => _inventoryData;
        
        private float _maxWeight;
        private float _weight;
        private bool _isOverweight;
        
        private BaseModifiersContainer _baseModifiersContainer;
        private ModifiersContainer _modifiersContainer;
        
        private ModifierData _overweightModifierData;
        
        #region ISaveble

        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_inventoryData);
        }

        public void Load(byte[] data)
        {
            _inventoryData = Serializer.Deserialize<InventoryData>(data);
        }

        #endregion

        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            _gameplayCharacterSaver.Load();

            _baseModifiersContainer = baseModifiersContainer;
            _modifiersContainer = modifiersContainer;

            _modifiersContainer.OnUpdateModifier -= OnUpdateModifier;
            _modifiersContainer.OnUpdateModifier += OnUpdateModifier;

            _overweightModifierData =
                new ModifierData(0.1f, float.PositiveInfinity, ModifierSpec.PercentageMultiplicative);

            _maxWeight = _modifiersContainer.GetValue(ModifierType.MaxWeight,
                _baseModifiersContainer.GetBaseValue(ModifierType.MaxWeight));
            _isOverweight = false;
            RecalculateWeight();
        }

        #region Items

        public void AddItem(InventoryItem item)
        {
            _inventoryData.AddItem(item);
            _gameplayCharacterSaver.Save();
            
            _weight += item.itemData.weight;
            
            OnOverweight(_weight > _maxWeight);
        }
        
        public void AddItem(ItemData itemData)
        {
            _inventoryData.AddItem(itemData);
            _gameplayCharacterSaver.Save();
            
            _weight += itemData.weight;
            
            OnOverweight(_weight > _maxWeight);
        }
        
        public void RemoveItem(InventoryItem item)
        {
            _inventoryData.RemoveItem(item);
            _gameplayCharacterSaver.Save();
            
            _weight -= item.itemData.weight;
            
            OnOverweight(_weight > _maxWeight);
        }
        
        public void RemoveItem(ItemData itemData)
        {
            _inventoryData.RemoveItem(itemData);
            _gameplayCharacterSaver.Save();
            
            _weight -= itemData.weight;
            
            OnOverweight(_weight > _maxWeight);
        }
        
        public void EquipItem(EquippableItem item)
        {
            item.isEquipped = true;
            OnEquipItem?.Invoke(item);
            
            _gameplayCharacterSaver.Save();
        }
        
        public void UnequipItem(EquippableItem item)
        {
            item.isEquipped = false;
            OnUnequipItem?.Invoke(item);
            
            _gameplayCharacterSaver.Save();
        }
        
        private void RecalculateWeight()
        {
            _weight = 0;
            
            foreach (var item in _inventoryData.GetAllItems())
            {
                _weight += item.itemData.weight;
            }
            
            OnOverweight(_weight > _maxWeight);
        }
        
        private void OnOverweight(bool isOverweight)
        {
            
            if (_isOverweight == isOverweight) return;
            
            if (isOverweight)
            {
                _modifiersContainer.Add(ModifierType.MoveSpeed, _overweightModifierData);
            }
            else
            {
                _modifiersContainer.Remove(ModifierType.MoveSpeed, _overweightModifierData);
            }
            
            _isOverweight = isOverweight;
            Debug.Log($"Is Overweight: {_isOverweight}");
        }

        #endregion

        #region Currency

        public void AddCurrency(int amount)
        {
            _inventoryData.currency += amount;
            _gameplayCharacterSaver.Save();
        }
        
        public bool TryRemoveCurrency(int amount)
        {
            if (_inventoryData.currency < amount)
            {
                return false;
            }
            
            _inventoryData.currency -= amount;
            _gameplayCharacterSaver.Save();
            return true;
        }
        
        public bool CheckCurrencyAmount(int amount)
        {
            return amount <= _inventoryData.currency;
        }

        #endregion
        
        private void OnUpdateModifier(ModifierType type)
        {
            if (type != ModifierType.MaxWeight) return;

            _maxWeight = _modifiersContainer.GetValue(ModifierType.MaxWeight,
                _baseModifiersContainer.GetBaseValue(ModifierType.MaxWeight));
            
            OnOverweight(_weight > _maxWeight);
        }
    }
}