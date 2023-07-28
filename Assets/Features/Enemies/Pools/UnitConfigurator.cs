using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Enemies.Pools
{
    [CreateAssetMenu(fileName = "UnitConfigurator", menuName = "Unit/UnitConfigurator")]
    public class UnitConfigurator : ScriptableObject
    {
        public UnitConfiguratorData[] units;

        private Dictionary<UnitType, Dictionary<int, List<UnitBehaviour>>> _container;

        public void Construct()
        {
            _container = new Dictionary<UnitType, Dictionary<int, List<UnitBehaviour>>>();
            for (var i = 0; i < units.Length; i++)
            {
                var unit = units[i].unit;
                if (!_container.ContainsKey(unit.Type))
                {
                    _container[unit.Type] = new Dictionary<int, List<UnitBehaviour>>();
                }
                
                if (!_container[unit.Type].ContainsKey(unit.Level))
                {
                    _container[unit.Type][unit.Level] = new List<UnitBehaviour>();
                }
                
                if (_container[unit.Type][unit.Level].Contains(unit))
                {
                    continue;
                }

                _container[unit.Type][unit.Level].Add(unit);
            }
        }

        public UnitBehaviour Get(UnitType type, int level)
        {
            var contains = _container.ContainsKey(type);
            if (!contains)
            {
                Debug.LogError($"Unit type {type} is not registered", this);
                return null;
            }
            
            var typeContainer = _container[type];
            contains = typeContainer.ContainsKey(level);
            if (!contains)
            {
                Debug.LogError($"Unit type {type} with level {level} is not registered", this);
                return null;
            }
            
            var levelContainer = typeContainer[level];
            var index = UnityEngine.Random.Range(0, levelContainer.Count);

            return levelContainer[index];
        }

        [Serializable]
        public class UnitConfiguratorData
        {
            public UnitBehaviour unit;
            public int count;
        }
    }
}
