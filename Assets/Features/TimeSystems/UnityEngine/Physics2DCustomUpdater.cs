using Features.ServiceLocators.Core.Service;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.TimeSystems.UnityEngine
{
    public class Physics2DCustomUpdater : MonoBehaviour, IFixedUpdateHandler
    {
        private SimulationMode2D _simulationMode2D;
        
        private void OnEnable()
        {
            _simulationMode2D = Physics2D.simulationMode;
            Physics2D.simulationMode = SimulationMode2D.Script;
        }
        
        private void OnDisable()
        {
            Physics2D.simulationMode = _simulationMode2D;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            Physics2D.Simulate(deltaTime);
        }
    }
}
