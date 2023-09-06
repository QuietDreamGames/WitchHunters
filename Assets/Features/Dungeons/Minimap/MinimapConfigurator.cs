using Edgar.Unity;
using UnityEngine;

namespace Features.Dungeons.Minimap
{
    [CreateAssetMenu(menuName = "Dungeon/Minimap Configurator", fileName = "MinimapConfigurator")]
    public class MinimapConfigurator : DungeonGeneratorPostProcessingGrid2D
    {
        [SerializeField] private MinimapPostProcessGrid2D minimapPostProcess;

        [SerializeField, Layer] private int minimapLayer;

        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            var layer = minimapLayer;

            if (layer == -1)
            {
                Debug.LogError("Error: wrong layer mask!");
                return;
            }

            if (minimapPostProcess != null)
            {
                minimapPostProcess.Layer = layer;
            }
        }
    }
}
