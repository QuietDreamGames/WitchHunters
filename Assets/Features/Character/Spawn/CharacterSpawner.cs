using System;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private CombatCharacterController _characterPrefab;

        [Header("DEBUG")] 
        [SerializeField] private bool spawnOnStart;

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnCharacter();
            }
        }

        public CombatCharacterController SpawnCharacter()
        {
            return InitiateCharacter(transform.position);
        }
        
        public CombatCharacterController SpawnCharacter(Vector3 position)
        {
            return InitiateCharacter(position);
        }

        private CombatCharacterController InitiateCharacter(Vector3 position)
        {
            var character = Instantiate(_characterPrefab, position, Quaternion.identity);
            character.Initiate();
            return character;
        }
    }
}