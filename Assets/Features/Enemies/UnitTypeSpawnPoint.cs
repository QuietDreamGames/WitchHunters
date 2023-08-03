using System;
using Features.Activator;
using Features.Enemies.Pools;
using Features.GameManagers;
using Features.ObjectPools.Core;
using Features.ServiceLocators.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Enemies
{
    public class UnitTypeSpawnPoint : MonoBehaviour, IActivator
    {
        [SerializeField] private UnitType type;
        [SerializeField] private Vector2Int levelRange;
        
        [Header("DEBUG")]
        [SerializeField] private bool spawnOnStart;

        private UnitConfigurator _unitConfigurator;
        private GameObjectPool<UnitBehaviour> _unitPool;
        private GameManager _gameManager;
        
        private UnitBehaviour _unit;

        public void Start()
        {
            _unitConfigurator = ServiceLocator.Resolve<UnitConfigurator>();
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
            var (min, max) = (levelRange.x, levelRange.y);
            var level = min == max ? min : Random.Range(min, max + 1);
            var unitPrefab = _unitConfigurator.Get(type, level);
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
