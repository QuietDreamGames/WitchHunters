using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Localization
{
    public class LocalizationInstaller : ServiceInstaller
    {
        [SerializeField] private LocalizationManager localizationManager;

        public override void Install()
        {
            var exist = ServiceLocator.Resolve<LocalizationManager>() != null;
            if (exist)
            {
                return;
            }
            
            ServiceLocator.Register<LocalizationManager>(localizationManager);
            
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}
