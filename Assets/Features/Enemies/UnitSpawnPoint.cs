using System;
using Features.Activator;
using Features.GameManagers;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Enemies
{
    public class UnitSpawnPoint : MonoBehaviour, IActivator
    {
        [SerializeField] private UnitBehaviour unitPrefab;
        
        [Header("DEBUG")]
        [SerializeField] private bool spawnOnStart;
        
        private GameObjectPool<UnitBehaviour> _unitPool;
        private GameManager _gameManager;
        
        private UnitBehaviour _unit;

        public void Start()
        {
            _unitPool = ServiceLocator.Resolve<GameObjectPool<UnitBehaviour>>();
            _gameManager = ServiceLocator.Resolve<GameManager>();
            if (_unitPool == null)
            {
                Debug.LogError("Unit pool is not registered", this);
                return;
            }
            
            if (spawnOnStart)
            {
                Activate();
            }
            
            if (_gameManager != null)
            {
                _gameManager.OnSceneChange += Deactivate;
            }
        }
        
        public void OnDestroy()
        {
            if (_gameManager != null)
            {
                _gameManager.OnSceneChange -= Deactivate;
            }
        }
        
        private void Spawn()
        {
            _unit = _unitPool.Spawn(unitPrefab.gameObject, transform);
            _unit.Pool = _unitPool;
            _unit.Prefab = unitPrefab.gameObject;
            
            _unit.transform.position = transform.position;
            
            _unit.Spawn();
        }

        public void Activate()
        {
            Spawn();
        }

        public void Deactivate()
        {
            if (_unit != null)
            {
                if (_unit.IsEnabled)
                {
                    _unit.Despawn();
                }
            }
        }
    }
}
