using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using Packages.OSSC.Scripts.Core;
using UnityEngine;

namespace Packages.OSSC.Scripts
{
    public class SoundControllerInstaller : ServiceInstaller
    {
        [SerializeField] private SoundController soundController;
        
        public override void Install()
        {
            var existSoundController = ServiceLocator.Resolve<SoundController>() != null;
            if (existSoundController)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ServiceLocator.Register<SoundController>(soundController);
            
            soundController.transform.parent = null;
            DontDestroyOnLoad(soundController);
        }
    }
}
