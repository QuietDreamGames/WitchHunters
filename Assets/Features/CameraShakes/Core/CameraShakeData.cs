using System;
using UnityEngine;

namespace Features.CameraShakes.Core
{
    [Serializable]
    public class CameraShakeData
    {
        public float duration = 1;
        public float amplitude = 1;
        public float frequency = 40;
        public ShakeDirection shakeDirection = ShakeDirection.XY;
        
        #if UNITY_EDITOR
        [SerializeField] private string comment;
        #endif

        public void RegisterShakeData(IShakeDirector shakeDirector)
        {
            shakeDirector.RegisterShakeData(this);
        }
        
        public void UnregisterShakeData(IShakeDirector shakeDirector)
        {
            shakeDirector.UnregisterShakeData(this);
        }
    }
}
