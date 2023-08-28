using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.TickUpdater
{
    public class TickUpdateCollect : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private int ticks = 20;
        
        [Header("Dependencies")]
        [SerializeField] private MonoBehaviour customTickUpdate;
        
        private ITickUpdateHandler _tickUpdateHandler;
        
        private float _tickRate;
        private float _tickTimer;
        
        private void Awake()
        {
            if (customTickUpdate != null)
            {
                _tickUpdateHandler = customTickUpdate as ITickUpdateHandler;
            }
            
            if (_tickUpdateHandler == null)
            {
                _tickUpdateHandler = GetComponent<ITickUpdateHandler>();
            }
            
            _tickRate = 1f / ticks;
            _tickTimer = 0f;
        }
        
        public void OnUpdate(float deltaTime)
        {
            _tickTimer += deltaTime;
            do 
            {
                _tickTimer -= _tickRate;
                _tickUpdateHandler.OnTickUpdate(_tickRate);
            } while (_tickTimer > _tickRate);
        }
    }
}
