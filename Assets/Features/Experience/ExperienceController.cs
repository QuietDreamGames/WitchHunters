using System;
using Features.Character;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Experience
{
    public class ExperienceController : MonoBehaviour, ISavable
    {
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;
        
        [SerializeField] private ExpData _expData;
        public int ExpAmount => _expData.expAmount;
        
        #region ISaveble

        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_expData);
        }

        public void Load(byte[] data)
        {
            _expData = Serializer.Deserialize<ExpData>(data);
        }

        #endregion

        public void Initiate()
        {
            _gameplayCharacterSaver.Load();
        }
        
        public void AddExp(int expAmount)
        {
            _expData.expAmount += expAmount;
            _gameplayCharacterSaver.Save();
        }
        
        public void SetExp(int expAmount)
        {
            _expData.expAmount = expAmount;
        }
    }

    [Serializable]
    public class ExpData
    {
        public int expAmount;
    }
}