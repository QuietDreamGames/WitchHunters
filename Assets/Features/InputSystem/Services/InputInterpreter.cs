using System.Collections.Generic;
using UnityEngine;

namespace Features.InputSystem.Services
{
    public class InputInterpreter
    {
        private Dictionary<string, Vector3> _axesMap;
        private Dictionary<string, bool> _keysMap;

        public InputInterpreter()
        {
            _axesMap = new Dictionary<string, Vector3>();
            _keysMap = new Dictionary<string, bool>();
        }

        public Vector3 GetAxis(string actionName)
        {
            return _axesMap.ContainsKey(actionName) ? _axesMap[actionName] : Vector3.zero;
        }

        public void SetAxis(string actionName, Vector3 value)
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