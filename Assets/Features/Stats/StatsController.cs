using System.Collections.Generic;
using Features.Character;
using Features.Experience;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Stats
{
    public class StatsController : MonoBehaviour, ISavable
    {
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;
        [SerializeField] private StatsData _statsData;
        
        public StatsData StatsData => _statsData;
        
        private ModifiersContainer _modifiersContainer;
        
        private Dictionary<ModifierType, ModifierData> _modifiersData = new ();
        
        #region ISaveble

        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_statsData);
        }

        public void Load(byte[] data)
        {
            _statsData = Serializer.Deserialize<StatsData>(data);
        }

        #endregion
        
        public void Initiate(LevelController levelController, ModifiersContainer modifiersContainer)
        {
            _gameplayCharacterSaver.Load();
            _modifiersContainer = modifiersContainer;
            _modifiersData = new Dictionary<ModifierType, ModifierData>();
            
            SetupStats();

            levelController.OnLevelUp += OnLevelUp;
        }
        
        private void SetupStats()
        {
            
            foreach (KeyValuePair<ModifierType, ModifierData> entry in _modifiersData)
            {
                _modifiersContainer.Remove(entry.Key, entry.Value);
            }
            
            _modifiersData.Clear();
            
            _modifiersData.Add(ModifierType.MaximumHealth,
                new ModifierData(_statsData.strength, float.PositiveInfinity, ModifierSpec.RawAdditional));
            _modifiersData.Add(ModifierType.AttackDamage,
                new ModifierData(_statsData.strength, float.PositiveInfinity, ModifierSpec.RawAdditional));
            
            _modifiersData.Add(ModifierType.AttackSpeed,
                new ModifierData(_statsData.agility * 0.01f, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            _modifiersData.Add(ModifierType.CastSpeed,
                new ModifierData(_statsData.agility * 0.01f, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            _modifiersData.Add(ModifierType.CriticalChance,
                new ModifierData(_statsData.agility * 0.5f, float.PositiveInfinity, ModifierSpec.RawAdditional));
            _modifiersData.Add(ModifierType.CriticalDamage,
                new ModifierData(_statsData.agility * 0.5f, float.PositiveInfinity, ModifierSpec.RawAdditional));
            
            _modifiersData.Add(ModifierType.PassiveChargedAttackDamage,
                new ModifierData(_statsData.intelligence, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            _modifiersData.Add(ModifierType.SecondarySkillDamage,
                new ModifierData(_statsData.intelligence, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            _modifiersData.Add(ModifierType.UltimateDamage,
                new ModifierData(_statsData.intelligence, float.PositiveInfinity, ModifierSpec.PercentageAdditional));
            _modifiersData.Add(ModifierType.MaximumShieldHealth,
                new ModifierData(_statsData.intelligence, float.PositiveInfinity, ModifierSpec.RawAdditional));

            foreach (KeyValuePair<ModifierType, ModifierData> entry in _modifiersData)
            {
                _modifiersContainer.Add(entry.Key, entry.Value);
            }
        }
        
        private void OnLevelUp()
        {
            AddUnusedStatsPoints(3);
        }
        
        public void AddUnusedStatsPoints(int statsPoints)
        {
            _statsData.unusedStatsPoints += statsPoints;
            _gameplayCharacterSaver.Save();
        }
        
        // public void RemoveUnusedStatPoints(int points)
        // {
        //     if (_statsData.unusedStatsPoints < points)
        //     {
        //         Debug.LogError("Not enough unused stats points");
        //         return;
        //     }
        //     
        //     _statsData.unusedStatsPoints -= points;
        //     _gameplayCharacterSaver.Save();
        // }
        //
        // public void AddStrength(int points)
        // {
        //     _statsData.strength += points;
        //     _gameplayCharacterSaver.Save();
        // }
        //
        // public void AddAgility(int points)
        // {
        //     _statsData.agility += points;
        //     _gameplayCharacterSaver.Save();
        // }
        //
        // public void AddIntelligence(int points)
        // {
        //     _statsData.intelligence += points;
        //     _gameplayCharacterSaver.Save();
        // }

        public void SetStatsData(StatsData statsData)
        {
            _statsData = statsData;
            _gameplayCharacterSaver.Save();
            
            SetupStats();
        }
    }
}