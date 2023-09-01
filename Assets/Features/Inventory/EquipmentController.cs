using Features.Character;
using Features.Inventory.Data;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.SaveSystems.Interfaces;
using UnityEngine;

namespace Features.Inventory
{
    public class EquipmentController : MonoBehaviour, ISavable
    {
        [SerializeField] private GameplayCharacterSaver _gameplayCharacterSaver;
        [SerializeField] private EquipmentData _equipmentData;
        
        private ModifiersContainer _modifiersContainer;
        private BaseModifiersContainer _baseModifiersContainer;
        
        #region ISaveble

        public ISavableSerializer Serializer { get; set; }
        public byte[] Save()
        {
            return Serializer.Serialize(_equipmentData);
        }

        public void Load(byte[] data)
        {
            _equipmentData = Serializer.Deserialize<EquipmentData>(data);
        }

        #endregion
        
        public void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            _gameplayCharacterSaver.Load();
            
            _modifiersContainer = modifiersContainer;
            _baseModifiersContainer = baseModifiersContainer;
            
            RecalculateModifiers();
        }

        private void RecalculateModifiers()
        {
            
        }
    }
}