using System;
using Features.Inventory;
using Features.Inventory.Data;
using Features.Inventory.Item;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using UnityEngine;

namespace Features.Drop
{
    [CreateAssetMenu(fileName = "DropData", menuName = "Drop", order = 0)]
    public class DropData : ScriptableObject
    {
        public DropItem[] possibleDrops;
        public DropModifierArmor[] possibleArmorModifiers;
        public DropModifierJewelry[] possibleJewelryModifiers;
        
        public int minimumCurrency;
        public int maximumCurrency;
        public float currencyDropChance;
        
        public int minimumExperience;
        public int maximumExperience;
        
        public InventoryItem GetRandomDrop()
        {
            var random = UnityEngine.Random.Range(0f, 1f);
            Debug.Log($"random: {random}");
            var currentChance = 1f;
            foreach (var drop in possibleDrops)
            {
                currentChance -= drop.dropChance;
                if (random < currentChance) continue;
                
                switch (drop.itemData.sortType)
                {
                    case ItemSortType.Armor:
                        var newItem = new EquippableItem
                        {
                            amount = 1,
                            isEquipped = false,
                            itemData = drop.itemData,
                        };
                            
                        newItem.ResetModifiers();
                        
                        var modifierRandom = UnityEngine.Random.Range(0f, 1f);
                        Debug.Log($"modifierRandom: {modifierRandom}");
                        var modifierCurrentChance = 1f;
                        for (int i = 0; i < possibleArmorModifiers.Length; i++)
                        {
                            var modifier = possibleArmorModifiers[i];
                            modifierCurrentChance -= modifier.dropChance;
                            if (modifierRandom < modifierCurrentChance) continue;

                            var itemModifier = new ItemModifier
                            {
                                modifierData = modifier.modifierData,
                                modifierType = modifier.modifierType
                            };
                            
                            newItem.itemModifiers.Add(itemModifier);
                            return newItem;
                        }
                        return newItem;
                    case ItemSortType.Jewelry:
                        newItem = new EquippableItem
                        {
                            amount = 1,
                            isEquipped = false,
                            itemData = drop.itemData,
                        };
                            
                        newItem.ResetModifiers();
                        
                        modifierRandom = UnityEngine.Random.Range(0f, 1f);
                        modifierCurrentChance = 1f;
                        for (int i = 0; i < possibleJewelryModifiers.Length; i++)
                        {
                            var modifier = possibleJewelryModifiers[i];
                            modifierCurrentChance -= modifier.dropChance;
                            if (modifierRandom < modifierCurrentChance) continue;

                            var itemModifier = new ItemModifier
                            {
                                modifierData = modifier.modifierData,
                                modifierType = modifier.modifierType
                            };
                            
                            newItem.itemModifiers.Add(itemModifier);
                            return newItem;
                        }
                        
                        return newItem;
                    case ItemSortType.Scrap:
                    case ItemSortType.Quest:
                        return new InventoryItem
                        {
                            amount = 1,
                            itemData = drop.itemData
                        };
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
            }

            return null;
        }
        
        public int GetExperience()
        {
            return UnityEngine.Random.Range(minimumExperience, maximumExperience);
        }
        
        public int GetCurrency()
        {
            var random = UnityEngine.Random.Range(0f, 1f);
            if (random < currencyDropChance) return 0;
            
            return UnityEngine.Random.Range(minimumCurrency, maximumCurrency);
        } 
    }
    
    [Serializable]
    public class DropItem
    {
        public ItemData itemData;
        public float dropChance;
    }
    
    [Serializable]
    public class DropModifierArmor
    {
        public ModifierType modifierType;
        public ModifierData modifierData;
        public float dropChance;
    }
    
    [Serializable]
    public class DropModifierJewelry
    {
        public ModifierType modifierType;
        public ModifierData modifierData;
        public float dropChance;
    }
}