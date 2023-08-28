using System;
using Features.Input;
using Features.ServiceLocators.Core;
using Features.TickUpdater;
using Features.TimeSystems.Interfaces.Handlers;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Network
{
    public class NetworkInput : NetworkBehaviour, IUpdateHandler, ITickUpdateHandler
    {
        #if UNITY_EDITOR
        [SerializeField]
        #endif
        private NetworkInputData _inputData;
        
        private PlayerInput _playerInput;
        private NetworkInputStorage _networkInputStorage;

        private bool _isActive = true;
        private bool _isSent;
        
        public NetworkInputData InputData => _inputData;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (IsOwner)
            {
                _playerInput = ServiceLocator.Resolve<InputData>().playerInput;
            }
            
            Debug.Log($"NetworkInput: {OwnerId}");
            
            _networkInputStorage = ServiceLocator.Resolve<NetworkInputStorage>();
            _networkInputStorage.Add(OwnerId, this);
        }
        
        public override void OnStopClient()
        {
            _networkInputStorage.Remove(OwnerId);
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (IsOwner)
            {
                if (_isSent)
                {
                    _inputData.Reset();
                    _isSent = false;
                }

                if (!_isActive)
                {
                    return;
                }
                
                _inputData.move = _playerInput.actions["Move"].ReadValue<Vector2>();
                
                _inputData.attack |= _playerInput.actions["Attack"].IsPressed();
                _inputData.secondary |= _playerInput.actions["Secondary"].IsPressed();
                _inputData.ultimate |= _playerInput.actions["Ultimate"].IsPressed();
                _inputData.shield |= _playerInput.actions["Shield"].IsPressed();
                
                _inputData.interact |= _playerInput.actions["Interact"].IsPressed();
            }
        }

        public void OnTickUpdate(float deltaTime)
        {
            if (IsOwner)
            {
                SendInputData(_inputData);
                _isSent = true;
            }
        }
        
        [ServerRpc]
        private void SendInputData(NetworkInputData inputData)
        {
            ReceiveInputData(inputData);
        }
        
        [ObserversRpc]
        private void ReceiveInputData(NetworkInputData inputData)
        {
            if (IsOwner) 
                return;
            _inputData = inputData;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _isActive = hasFocus;
        }
    }

    [Serializable]
    public struct NetworkInputData
    {
        public Vector2 move;
        
        public bool attack;
        public bool secondary;
        public bool ultimate;
        public bool shield;
        
        public bool interact;
        
        public void Reset()
        {
            move = Vector2.zero;
            
            attack = false;
            secondary = false;
            ultimate = false;
            shield = false;
            
            interact = false;
        }
    }
}
