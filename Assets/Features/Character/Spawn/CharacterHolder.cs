using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterHolder : NetworkBehaviour
    {
        [SerializeField] private CharacterData[] characters;
        
        private CharacterData _currentCharacterData;
        
        public CombatCharacterController CurrentCharacter => _currentCharacterData.character;
        
        public Action<CombatCharacterController> OnCharacterChanged;

        public void SetCharacter(CharacterType type)
        {
            SetCharacterServer(type, this, LocalConnection);
        }

        [ServerRpc]
        public void SetCharacterServer(CharacterType type, CharacterHolder self, NetworkConnection conn)
        {
            if (_currentCharacterData != null)
            {
                SetActiveCharacter(false, _currentCharacterData);
            }
            
            var characterData = GetCharacterData(type);
            if (characterData.character == null)
            {
                characterData.character = Instantiate(characterData.prefab, transform, false);
                ServerManager.Spawn(characterData.character.gameObject, conn);
            }
            else
            {
                SetActiveCharacter(true, characterData);
            } 
            
            SetCharacterObservers(characterData, conn);
        }
        
        [ObserversRpc]
        public void SetCharacterObservers(CharacterData serverData, NetworkConnection conn)
        {
            if (conn != LocalConnection)
                return;
            
            var characterData = GetCharacterData(serverData.type);
            characterData.character = serverData.character;
            
            characterData.character.Initiate();
            
            _currentCharacterData = characterData;
            
            OnCharacterChanged?.Invoke(_currentCharacterData.character);
        }
        
        public CharacterData GetCharacterData(CharacterType type)
        {
            foreach (var character in characters)
            {
                if (character.type == type)
                {
                    return character;
                }
            }

            Debug.LogError($"Character with type {type} not found", this);
            return characters[0];
        }
        
        public static void SetActiveCharacter(bool active, CharacterData characterData)
        {
            characterData.character.gameObject.SetActive(active);
        }

        [Serializable]
        public class CharacterData
        {
            public CombatCharacterController prefab;
            public CharacterType type;

            [HideInInspector] 
            public CombatCharacterController character;
        }
    }
}