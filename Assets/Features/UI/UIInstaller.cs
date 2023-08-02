using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.UI
{
    public class UIInstaller : ServiceInstaller
    {
        [SerializeField] private LoadingScreenUI loadingScreenUIPrefab;
        
        public override void Install()
        {
            var loadingScreenUI = Instantiate(loadingScreenUIPrefab);
            ServiceLocator.Register<LoadingScreenUI>(loadingScreenUI);
        }
    }
}
