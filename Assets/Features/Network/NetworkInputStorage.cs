using System.Collections.Generic;
using UnityEngine;

namespace Features.Network
{
    public class NetworkInputStorage
    {
        private readonly Dictionary<int, NetworkInput> _networkInputs = new(4);
        
        public void Add(int id, NetworkInput networkInput)
        {
            _networkInputs[id] = networkInput;
        }
        
        public void Remove(int id)
        {
            var exist = _networkInputs.TryGetValue(id, out _);
            if (!exist) 
                return;
            _networkInputs.Remove(id);
        }
        
        public NetworkInput Get(int id)
        {
            var exist = _networkInputs.TryGetValue(id, out var networkInput);
            if (!exist) 
                return null;
            return networkInput;
        }
    }
}
