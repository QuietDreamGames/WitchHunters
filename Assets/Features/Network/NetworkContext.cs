using Features.ServiceLocators.Core.Service;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Features.Network
{
    public class NetworkContext : NetworkBehaviour
    {
        [SerializeField] private ServiceInstaller[] installers;
        
        public override void OnStartClient()
        {
            if (IsOwner)
            {
                Install();
            }
        }
        
        private void Install()
        {
            Debug.Log($"Install: {gameObject.name}", this);
            
            for (var i = 0; i < installers.Length; i++)
            {
                installers[i].Install();
            }
        }
    }
}
