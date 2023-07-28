using Unity.Collections;
using UnityEngine;

namespace Features.Enemies.Steering
{
    public class ContextSteering : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int rayCount = 16;
        
        [Header("Debug")] 
        [SerializeField, ReadOnly] private Vector2 steering;
        
        private Vector2[] _directions;
        private float[] _interests;
        private float[] _dangers;
        
        private const float Epsilon = 0.003f;

        private void Start()
        {
            _directions = new Vector2[rayCount];
            _interests = new float[rayCount];
            _dangers = new float[rayCount];
            
            var rayAngle = 360f / rayCount;
            for (var i = 0; i < rayCount; i++)
            {
                var angle = i * rayAngle;
                _directions[i] = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            }
        }
        
        public void ResetDanger()
        {
            for (var i = 0; i < rayCount; i++)
            {
                _dangers[i] = 0;
            }
        }
        
        public void AddDanger(Vector2 dangerRay)
        {
            for (var i = 0; i < rayCount; i++)
            {
                var dot = Vector2.Dot(dangerRay, _directions[i]);

                if (dot > 0)
                {
                    _dangers[i] = dot;
                }
                else
                {
                    _dangers[i] = 0;
                }
            }
        }

        public void SetInterest(Vector2 interestRay)
        {
            for (var i = 0; i < rayCount; i++)
            {
                var dot = Vector2.Dot(interestRay, _directions[i]);
                
                _interests[i] = (dot + 1) / 2;
            }
        }
        
        public bool IsDangerDirection(Vector2 direction)
        {
            var maxDot = Epsilon;
            var maxIndex = -1;
            for (var i = 0; i < rayCount; i++)
            {
                var dot = Vector2.Dot(direction, _directions[i]);
                if (dot > maxDot)
                {
                    maxDot = dot;
                    maxIndex = i;
                }
            }

            return maxIndex >= 0 && _dangers[maxIndex] > Epsilon;
        }
        
        public Vector2 GetSteering()
        {
            steering = Vector2.zero;
            var bestMultiplier = Epsilon;
            
            for (var i = 0; i < rayCount; i++)
            {
                var interest = _interests[i];
                var danger = _dangers[i];
                var direction = _directions[i];
                
                var multiplier = interest - danger;
                if (multiplier <= bestMultiplier)
                {
                    continue;
                }
                
                steering = (direction * multiplier).normalized;
                bestMultiplier = multiplier;
            }
            
            return steering;
        }
    }
}
