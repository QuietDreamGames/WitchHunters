using Edgar.Unity;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Progression
{
    public class LevelGraphConfigurator : MonoBehaviour
    {
        [SerializeField] private DungeonGeneratorGrid2D generator;

        public void ConfigureLevelGraph()
        {
            var gameProgression = ServiceLocator.Resolve<GameProgression>();
            var levelGraph = gameProgression.GetLevelGraph();
            
            generator.FixedLevelGraphConfig.LevelGraph = levelGraph;
        }
    }
}
