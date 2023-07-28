using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Floor
{
    public class FloorControllerInstaller : ServiceInstaller
    {
        [SerializeField] private FloorController _floorController;
        public override void Install()
        {
            ServiceLocator.Register<FloorController>(_floorController);
        }
    }
}