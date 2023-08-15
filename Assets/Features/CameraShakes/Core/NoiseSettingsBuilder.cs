using Cinemachine;
using UnityEngine;

namespace Features.CameraShakes.Core
{
    public class NoiseSettingsBuilder : MonoBehaviour
    {
        public NoiseSettings x;
        public NoiseSettings y;
        public NoiseSettings xy;
        
        public NoiseSettings Build(ShakeDirection shakeDirection)
        {
            switch (shakeDirection)
            {
                case ShakeDirection.X:
                    return x;
                case ShakeDirection.Y:
                    return y;
                case ShakeDirection.XY:
                    return xy;
            }
            
            return null;
        }
    }
}