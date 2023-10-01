using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Progression
{
    public class GameProgressionInstaller : ServiceInstaller
    {
        [SerializeField] private GameProgression gameProgressionPrefab;

        public override void Install()
        {
            var exist = ServiceLocator.Resolve<GameProgression>() != null;
            if (exist)
            {
                gameObject.SetActive(false);
                return;
            }
            
            var self = transform;
            var gameProgression = Instantiate<GameProgression>(gameProgressionPrefab, self);
            
            ServiceLocator.Register<GameProgression>(gameProgression);

            self.parent = null;
            DontDestroyOnLoad(self);
        }
    }
}
