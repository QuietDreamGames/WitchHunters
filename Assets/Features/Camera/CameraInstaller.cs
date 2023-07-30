using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Camera
{
    public class CameraInstaller : ServiceInstaller
    {
        [SerializeField] private CameraData cameraData;
        
        public override void Install()
        {
            ServiceLocator.Register<CameraData>(cameraData);
        }
    }
}
