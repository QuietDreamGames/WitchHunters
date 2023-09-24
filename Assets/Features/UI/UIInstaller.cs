using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using Features.UI.Gameplay;
using UnityEngine;

namespace Features.UI
{
    public class UIInstaller : ServiceInstaller
    {
        [SerializeField] private LoadingScreenUI loadingScreenUIPrefab;
        [SerializeField] private GameplayCanvasContainer gameplayCanvasContainerPrefab;
        
        public override void Install()
        {
            var loadingScreenUI = Instantiate(loadingScreenUIPrefab, transform, false);
            ServiceLocator.Register<LoadingScreenUI>(loadingScreenUI);
            
            var gameplayCanvasContainer = Instantiate(gameplayCanvasContainerPrefab, transform, false);
            ServiceLocator.Register<GameplayCanvasContainer>(gameplayCanvasContainer);
        }
    }
}
