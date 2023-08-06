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
            ServiceLocator.Register<LocalizationManager>(localizationManager);
        }
    }
}
