using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Features.InputSystem.Services
{
    public class InputInterpreter
    {
        private Dictionary<string, float3> _axesMap;
        private Dictionary<string, bool> _keysMap;

        public InputInterpreter()
        {
            _axesMap = new Dictionary<string, float3>();
            _keysMap = new Dictionary<string, bool>();
        }

        public float3 GetAxis(string actionName)
        {
            return _axesMap.ContainsKey(actionName) ? _axesMap[actionName] : float3.zero;
        }

        public void SetAxis(string actionName, float3 value)
        {
            _axesMap[actionName] = value;
        }

        public bool GetKey(string keyName)
        {
            return _keysMap.ContainsKey(keyName) && _keysMap[keyName];
        }
        
        public void SetKey(string keyName, bool value)
        {
            _keysMap[keyName] = value;
        }
    }
}