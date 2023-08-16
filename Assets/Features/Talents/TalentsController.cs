using System.Collections.Generic;
using System.Linq;
using Features.Character;
using Features.Experience;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Talents
{
    public class TalentsController : MonoBehaviour, ISavable
    {
        // [SerializeField] private TalentsList _talentsList; // static list of all talents, learned and unlearned
        [SerializeField] private TalentsData _talentsData; // learned talents
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;

        private ModifiersContainer _modifiersContainer;
        private Dictionary<ModifierType, ModifierData> _modifiersData = new ();
        
        #region ISaveble
        
        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_talentsData);
        }

        public void Load(byte[] data)
        {
            _talentsData = Serializer.Deserialize<TalentsData>(data);
        }

        #endregion

        public void Initiate(LevelController levelController, ModifiersContainer modifiersContainer)
        {
            _gameplayCharacterSaver.Load();
            _modifiersContainer = modifiersContainer;
            
            _modifiersData = new Dictionary<ModifierType, ModifierData>();
            
            SetupTalents();
            
            levelController.OnLevelUp += OnLevelUp;
        }
        
        public void SetupTalents()
        {
            foreach (KeyValuePair<ModifierType, ModifierData> entry in _modifiersData)
            {
                _modifiersContainer.Remove(entry.Key, entry.Value);
            }
            
            _modifiersData.Clear();
            
            foreach (var talentDataEntry in _talentsData.learnedTalents.SelectMany(talent => talent.talentData))
            {
                _modifiersData.Add(talentDataEntry.modifierType, talentDataEntry.modifier);
            }
            
            foreach (KeyValuePair<ModifierType, ModifierData> entry in _modifiersData)
            {
                _modifiersContainer.Add(entry.Key, entry.Value);
            }
        }

        private void OnLevelUp()
        {
            AddTalentPoint();
        }

        public void LearnTalent(TalentData talentData)
        {
            _talentsData.learnedTalents.Add(talentData);
            SetupTalents();
            _gameplayCharacterSaver.Save();
        }

        public void AddTalentPoint()
        {
            _talentsData.talentPoints++;
            _gameplayCharacterSaver.Save();
        }
    }
}