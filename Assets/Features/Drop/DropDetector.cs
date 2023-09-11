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
        private bool _isLastDropDetected;
        
        public void OnFixedUpdate(float deltaTime)
        {
            DetectDrop();
        }
        
        private void DetectDrop()
        {
            if (_isLastDropDetected)
            {
                _lastDropInstance.SetDetected(false);
            }
            
            var colliders = new Collider2D[10];
            
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, colliders, _dropMask.value);
            if (count == 0)
            {
                _lastDropInstance = null;
                _isLastDropDetected = false;
                _lastDropCollider = null;
                return;
            }

            if (_lastDropCollider != colliders[0])
            {
                _lastDropInstance = colliders[0].GetComponent<DropInstance>();
            }
            
            _lastDropInstance.SetDetected(true);
            _isLastDropDetected = true;
        }

        public void OnDrawGizmosSelected()
        {
            var origin = transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, _radius);
        }
    }
}