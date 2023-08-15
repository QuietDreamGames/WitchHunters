using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.CameraShakes.Core
{
    public class ShakeDirectorInstaller : ServiceInstaller
    {
        [SerializeField] private MonoBehaviour shakeDirectorBehaviour;
        
        public override void Install()
        {
            var shakeDirector = shakeDirectorBehaviour as IShakeDirector;
            if (shakeDirector == null)
            {
                Debug.LogError($"ShakeDirectorInstaller: {shakeDirectorBehaviour} is not IShakeDirector");
                return;
            }
            
            ServiceLocator.Register<IShakeDirector>(shakeDirector);
        }
    }
}