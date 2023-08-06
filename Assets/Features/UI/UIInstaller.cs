using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.UI
{
    public class UIInstaller : ServiceInstaller
    {
        [SerializeField] private LoadingScreenUI loadingScreenUIPrefab;
        [SerializeField] private Canvas hudPrefab;
        
        public override void Install()
        {
            var loadingScreenUI = Instantiate(loadingScreenUIPrefab, transform, false);
            ServiceLocator.Register<LoadingScreenUI>(loadingScreenUI);
            
            var hud = Instantiate(hudPrefab, transform, false);
        }
    }
}
