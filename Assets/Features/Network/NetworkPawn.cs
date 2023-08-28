using System;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Features.Network
{
    public class NetworkPawn : NetworkBehaviour
    {
        private void Awake()
        {
            InstanceFinder.ClientManager.OnConnectedClients += OnConnectedClients;
        }
        
        private void OnDestroy()
        {
            InstanceFinder.ClientManager.OnConnectedClients -= OnConnectedClients;
        }

        private void OnConnectedClients(ConnectedClientsArgs args)
        {
            Debug.Log($"OnConnectedClients: {gameObject.name}", this);
        }
    }
}
