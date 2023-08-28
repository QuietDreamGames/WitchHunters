using System;
using Features.Camera;
using Features.ServiceLocators.Core;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterSpawnPoint : NetworkBehaviour
    {
        private CharacterHolder _characterHolder;
        private CameraData _cameraData;

        private void Awake()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _cameraData = ServiceLocator.Resolve<CameraData>();
            
            InstanceFinder.ClientManager.OnConnectedClients += OnConnectedClients;
        }
        
        private void OnDestroy()
        {
            InstanceFinder.ClientManager.OnConnectedClients -= OnConnectedClients;
        }

        private void OnConnectedClients(ConnectedClientsArgs args)
        {
            Debug.Log($"OnConnectedClients: {gameObject.name}", this);
            
            /*
            var character = _characterHolder.CurrentCharacter;
            var characterTransform = character.transform;
            characterTransform.position = transform.position;
            
            _cameraData.SetTarget(characterTransform); 
            */
        }
    }
}
