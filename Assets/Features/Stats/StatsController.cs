using Features.Character;
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
        
        public void Initiate(ModifiersContainer modifiersContainer)
        {
            _gameplayCharacterSaver.Load();
            _modifiersContainer = modifiersContainer;
            SetupStats();
        }
        
        private void SetupStats()
        {
            // _modifiersContainer.Add(ModifierType.MaximumHealth, ModifierSpec.RawAdditional, float.PositiveInfinity,
            //     _statsData.strength);
            // _modifiersContainer.Add(ModifierType.AttackDamage, ModifierSpec.RawAdditional, float.PositiveInfinity,
            //     _statsData.strength);
            //
            // _modifiersContainer.Add(ModifierType.AttackSpeed, ModifierSpec.PercentageAdditional, float.PositiveInfinity,
            //     _statsData.agility);
            // _modifiersContainer.Add(ModifierType.CastSpeed, ModifierSpec.PercentageAdditional, float.PositiveInfinity,
            //     _statsData.agility);
            // _modifiersContainer.Add(ModifierType.CriticalChance, ModifierSpec.RawAdditional, float.PositiveInfinity,
            //     _statsData.agility * 0.5f);
            // _modifiersContainer.Add(ModifierType.CriticalDamage, ModifierSpec.RawAdditional, float.PositiveInfinity,
            //     _statsData.agility * 0.5f);
            //
            // _modifiersContainer.Add(ModifierType.PassiveChargedAttackDamage, ModifierSpec.PercentageAdditional,
            //     float.PositiveInfinity, _statsData.intelligence);
            // _modifiersContainer.Add(ModifierType.SecondarySkillDamage, ModifierSpec.PercentageAdditional,
            //     float.PositiveInfinity, _statsData.intelligence);
            // _modifiersContainer.Add(ModifierType.UltimateDamage, ModifierSpec.PercentageAdditional,
            //     float.PositiveInfinity, _statsData.intelligence);
            // _modifiersContainer.Add(ModifierType.MaximumShieldHealth, ModifierSpec.RawAdditional,
            //     float.PositiveInfinity, _statsData.intelligence);
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
        }
    }
}