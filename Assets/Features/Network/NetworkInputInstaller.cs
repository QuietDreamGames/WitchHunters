using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Network
{
    public class NetworkInputInstaller : ServiceInstaller
    {
        public override void Install()
        {
            var networkInputStorage = new NetworkInputStorage();
            ServiceLocator.Register<NetworkInputStorage>(networkInputStorage);
        }
    }
}
