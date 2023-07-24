using System.Collections.Generic;
using UnityEngine;

namespace Features.ObjectPools.Core
{
    public class PrefabPool
    {
        private GameObject _prefab;

        private List<GameObject> _used;
        private List<GameObject> _free;
        
        private int _count;
        
        private Transform _parent;
        
        public PrefabPool(GameObject prefab, Transform root)
        {
            _prefab = prefab;

            var go = new GameObject($"{prefab.name}");
            go.SetActive(false);
            
            _parent = go.transform;
            _parent.SetParent(root, false);
            _parent.position = Vector3.zero;
            
            _used = new List<GameObject>(10);
            _free = new List<GameObject>(10);
            
            _count = 0;
        }

        public void Prewarm(int newCount)
        {
            var diff = newCount - _count;
            
            if (diff <= 0)
                return;
            
            CreateChildren(diff);
        }

        public GameObject Spawn()
        {
            if (_free.Count == 0)
                CreateChildren(1);

            var go = _free[0];
            _used.Add(go);
            _free.Remove(go);
            
            return go;
        }
        
        public void Despawn(GameObject go)
        {
            if (!_used.Contains(go))
            {
                Debug.LogError($"{go.name} is not in pool!");
                return;
            }
            
            if (_free.Contains(go))
            {
                Debug.LogError($"{go.name} is already in pool!");
                return;
            }
            
            go.transform.SetParent(_parent, false);
            go.SetActive(false);
            
            _free.Add(go);
            _used.Remove(go);
        }
        
        private void CreateChildren(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var go = Object.Instantiate(_prefab, _parent);
                go.SetActive(false);
                _free.Add(go);
                _count++;
            }
        }
    }
}
