﻿using System;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterHolder : MonoBehaviour
    {
        [SerializeField] private CharacterData[] characters;

        [Header("DEBUG")] 
        [SerializeField] private bool spawnOnStart;
        
        private CharacterData _currentCharacterData;
        
        public CombatCharacterController CurrentCharacter => _currentCharacterData.character;
        
        public Action<CombatCharacterController> OnCharacterChanged;
        
        private void Awake()
        {
            if (spawnOnStart)
            {
                SetCharacter(CharacterType.Inquisitor);
            }
        }

        public void SetCharacter(CharacterType type)
        {
            if (_currentCharacterData != null)
            {
                SetActiveCharacter(false, _currentCharacterData);
            }
            
            var characterData = GetCharacterData(type);
            if (characterData.character == null)
            {
                characterData.character = Instantiate(characterData.prefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                SetActiveCharacter(true, characterData);
            }
            
            characterData.character.Initiate();
            
            _currentCharacterData = characterData;
            
            OnCharacterChanged?.Invoke(_currentCharacterData.character);
        }
        
        private CharacterData GetCharacterData(CharacterType type)
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
        
        private static void SetActiveCharacter(bool active, CharacterData characterData)
        {
            characterData.character.gameObject.SetActive(active);
        }

        [Serializable]
        private class CharacterData
        {
            public CombatCharacterController prefab;
            public CharacterType type;

            [HideInInspector] 
            public CombatCharacterController character;
        }
    }
}