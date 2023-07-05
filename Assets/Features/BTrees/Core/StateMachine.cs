using System.Collections.Generic;
using Features.BTrees.Interfaces;
using UnityEngine;

namespace Features.BTrees.Core
{
    public class StateMachine : MonoBehaviour, IBTreeMachine
    {
        [SerializeField] private Node root;
        [SerializeField] private Object[] extensions;
        
        public T GetExtension<T>() where T : Object
        {
            for (var i = 0; i < extensions.Length; i++)
            {
                if (extensions[i] is T)
                {
                    return (T) extensions[i];
                }
            }
            
            return null;
        }

        public IEnumerable<T> GetExtensions<T>() where T : Object
        {
            var list = new List<T>();

            for (var i = 0; i < extensions.Length; i++)
            {
                if (extensions[i] is T)
                {
                    list.Add((T) extensions[i]);
                }
            }

            return list;
        }
        
        private void Start()
        {
            root.Construct(this);
        }
        
        private void Update()
        {
            root.UpdateCustom(Time.deltaTime);
        }
    }
}
