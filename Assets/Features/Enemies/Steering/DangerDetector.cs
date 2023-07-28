using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Enemies.Steering
{
    public class DangerDetector : MonoBehaviour, IFixedUpdateHandler
    {
        [Header("Radius")]
        [SerializeField] private float enterRadius = 1;
        [SerializeField] private float exitRadius = 1;
        
        [Header("Layer")]
        [SerializeField] private LayerMask dangerLayer;
        
        [Header("Dependencies")]
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private ContextSteering contextSteering;
        
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[10];
        
        private bool _isEnabled;

        #region IFixedUpdateHandler impleentation

        public void OnFixedUpdate(float deltaTime)
        {
            if (!_isEnabled)
            {
                return;
            }
            
            Detect();
        }

        #endregion
        
        #region MonoBehaviour
        
        private void OnEnable()
        {
            _isEnabled = true;
        }
        
        private void OnDisable()
        {
            _isEnabled = false;
        }
        
        #endregion

        #region Private methods

        private void Detect()
        {
            Vector2 origin = transform.position;
            var count = Physics2D.CircleCastNonAlloc(origin, exitRadius, Vector2.zero, _hits, 0, dangerLayer.value);
            
            contextSteering.ResetDanger();
            
            for (var i = 0; i < count; i++)
            {
                var hit = _hits[i];
                if (hit.collider == null)
                {
                    continue;
                }
                
                if (hit.collider.attachedRigidbody == rigidbody2D)
                {
                    continue;
                }
                
                var distance = Vector2.Distance(origin, hit.point);
                var coefficient = Mathf.InverseLerp(exitRadius, enterRadius, distance);
                var direction = hit.point - origin;
                contextSteering.AddDanger(direction.normalized * coefficient);
            }
        }

        #endregion



        public void OnDrawGizmosSelected()
        {
            var origin = transform.position;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, enterRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin, exitRadius);
        }
    }
}
