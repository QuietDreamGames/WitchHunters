using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private CombatCharacterController _characterPrefab;

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