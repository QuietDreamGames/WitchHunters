using System.Collections.Generic;
using System.Linq;
using Features.Character;
using Features.Experience;
using Features.Modifiers.SOLID.Core;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Talents
{
    public class TalentsController : MonoBehaviour, ISavable
    {
        [SerializeField] private TalentsList _talentsList; // static list of all talents, learned and unlearned
        [SerializeField] private TalentsData _talentsData; // learned talents
        
        public TalentsList TalentsList => _talentsList;
        public TalentsData TalentsData => _talentsData;
        
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;

        private ModifiersContainer _modifiersContainer;
        private List<TalentData> _talentModifiersData;
        
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
            
            _talentModifiersData = new List<TalentData>();
            
            SetupTalents();
            
            levelController.OnLevelUp += OnLevelUp;
        }
        
        public void SetupTalents()
        {
            foreach (var talentDataEntry in _talentModifiersData.SelectMany(talentData => talentData.talentData))
            {
                _modifiersContainer.Remove(talentDataEntry.modifierType, talentDataEntry.modifier);
            }
            
            _talentModifiersData = _talentsData.Clone();
            
            foreach (var entry in _talentModifiersData.SelectMany(talentData => talentData.talentData))
            {
                _modifiersContainer.Add(entry.modifierType, entry.modifier);
            }
        }

        private void OnLevelUp()
        {
            AddTalentPoint();
        }

        public void LearnTalent(TalentData talentData)
        {
            _talentsData.talentPoints--;
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