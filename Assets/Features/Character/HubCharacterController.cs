using System;
using Features.Hub;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character
{
    
    public class HubCharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        private PlayerInput _playerInput;
        
        private bool _canChangeCharacter;
        private HubCharacterController _controllerToChange;

        private bool _isActive;
        
        
        // public void Start()
        // {
        //     HubCharactersManager.Instance.RegisterCharacter(this);
        // }

        public void SetPlayerInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _characterController.SetPlayerInput(playerInput);
            _isActive = playerInput != null;
            _characterController.SetActive(_isActive);
            _canChangeCharacter = false;
        }
        
        private void AllowChangeCharacter(bool state, HubCharacterController controllerToChange)
        {
            if (state)
            {
                _canChangeCharacter = true;
                _controllerToChange = controllerToChange;
            }
            else
            {
                _canChangeCharacter = false;
                _controllerToChange = null;
            }
        }
        
        private void Update()
        {
            if (!_isActive) return;

            if (_canChangeCharacter && _playerInput.actions["Interact"].triggered)
            {
                HubCharactersManager.Instance.ChangeCurrentCharacter(_controllerToChange);
                _canChangeCharacter = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive) return;
            if (other.CompareTag("Player"))
            {
                AllowChangeCharacter(true, other.GetComponent<HubCharacterController>());
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_isActive) return;
            if (other.CompareTag("Player"))
            {
                AllowChangeCharacter(false, null);
            }
        }
    }
}