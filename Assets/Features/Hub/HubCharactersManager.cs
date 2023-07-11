using System.Collections.Generic;
using Features.Character;
using Features.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Hub
{
    public class HubCharactersManager : Singleton<HubCharactersManager>
    {
        [SerializeField] private PlayerInput _playerInput;
        
        [SerializeField] private HubCharacterController[] _characterPrefabs;
        [SerializeField] private Transform[] _spawnPoints;
        
        private HubCharacterController _currentCharacterController;
        private readonly List<HubCharacterController> _characters = new ();
        
        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < _characterPrefabs.Length; i++)
            {
                var character = Instantiate(_characterPrefabs[i], _spawnPoints[i].position, Quaternion.identity);
                _characters.Add(character);
                
                if (i == 0)
                {
                    ChangeCurrentCharacter(character);
                }
            }
        }
        
        // public void RegisterCharacter(HubCharacterController characterController)
        // {
        //     
        // }
        
        public void ChangeCurrentCharacter(HubCharacterController newCharacterController)
        {
            Debug.Log(".");
            _currentCharacterController = newCharacterController;
            
            for (int i = 0; i < _characters.Count; i++)
            {
                _characters[i].SetPlayerInput(_characters[i] == _currentCharacterController
                    ? _playerInput
                    : null);
            }
        }
        
    }
}