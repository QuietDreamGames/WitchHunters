using System.Collections.Generic;
using UnityEngine;

namespace Features.ObjectPools.Core
{
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private readonly Dictionary<GameObject, PrefabPool> _pools;
        
        private readonly Transform _parent;
        
        public GameObjectPool(Transform root)
        {
            _pools = new Dictionary<GameObject, PrefabPool>();
            
            var go = new GameObject($"ObjectPool::{typeof(T).Name}");
            
            _parent = go.transform;
            _parent.SetParent(root, false);
        }
        
        public void Prewarm(GameObject prefab, int newCount)
        {
            var pool = GetPool(prefab);
            pool.Prewarm(newCount);
        }
        
        public T Spawn(GameObject prefab, Transform newParent)
        {
            var pool = GetPool(prefab);
            var go = pool.Spawn();
            go.transform.SetParent(newParent, false);

            go.gameObject.SetActive(true);
            var component = go.GetComponent<T>();
            return component;
        }
        
        public void Despawn(GameObject prefab, T component)
        {
            var componentTransform = component.transform;
            componentTransform.SetParent(_parent, false);
            componentTransform.position = _parent.position;

            if (prefab == null)
            {
                Debug.LogError($"{prefab.name} is not in pool!");
                return;
            }
            
            var pool = GetPool(prefab);
            pool.Despawn(component.gameObject);
        }
        
        private PrefabPool GetPool(GameObject prefab)
        {
            if (!_pools.ContainsKey(prefab))
            {
                _pools[prefab] = new PrefabPool(prefab, _parent);
            }

            return _pools[prefab];
        }
    }
}