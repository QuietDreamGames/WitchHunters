using System;
using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.Drop
{
    public class DropDetector : MonoBehaviour, IFixedUpdateHandler
    {
        [SerializeField] private float _radius = 2f;
        [SerializeField] private LayerMask _dropMask;
        
        private Collider2D _lastDropCollider;
        private DropInstance _lastDropInstance; 

        public Action<bool, DropInstance> OnDropDetected;
        
        public void OnFixedUpdate(float deltaTime)
        {
            DetectDrop();
        }
        
        private void DetectDrop()
        {
            if (_lastDropInstance != null)
            {
                _lastDropInstance.SetDetected(false);
            }
            
            var colliders = new Collider2D[10];
            
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, colliders, _dropMask.value);
            if (count == 0)
            {
                _lastDropInstance = null;
                _lastDropCollider = null;
                OnDropDetected?.Invoke(false, _lastDropInstance);
                return;
            }
            
            var closest = FindClosest(colliders);

            if (_lastDropCollider != closest)
            {
                _lastDropCollider = closest;
                _lastDropInstance = _lastDropCollider.GetComponent<DropInstance>();
            }
            else if (_lastDropInstance == null)
            {
                _lastDropInstance = closest.GetComponent<DropInstance>();
            }
            
            _lastDropInstance.SetDetected(true);
            OnDropDetected?.Invoke(true, _lastDropInstance);
        }

        private Collider2D FindClosest(Collider2D[] collider2Ds)
        {
            var closest = collider2Ds[0];
            var closestDistance = Vector2.Distance(transform.position, closest.transform.position);
            
            for (var i = 1; i < collider2Ds.Length; i++)
            {
                if (collider2Ds[i] == null)
                    break;
                
                var distance = Vector2.Distance(transform.position, collider2Ds[i].transform.position);
                if (distance < closestDistance)
                {
                    closest = collider2Ds[i];
                    closestDistance = distance;
                }
            }

            return closest;
        }

        public void OnDrawGizmosSelected()
        {
            var origin = transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, _radius);
        }
    }
}