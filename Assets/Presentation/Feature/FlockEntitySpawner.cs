using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class FlockEntitySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _boidPrefab;
        [SerializeField] private int _count;
        
        [SerializeField] private Transform _center;
        [SerializeField] private Vector2 _size;
        
        [Header("Params")]
        [SerializeField] private float _separationWeight;
        [SerializeField] private float _alignmentWeight;
        [SerializeField] private float _cohesionWeight;
        
        [SerializeField] private float _separationMultiplier;
        [SerializeField] private float _alignmentMultiplier;
        [SerializeField] private float _cohesionMultiplier;
        
        [SerializeField] private float _maxSpeed;
        
        public float MaxSpeed => _maxSpeed;
        
        public float SeparationWeight => _separationWeight;
        public float AlignmentWeight => _alignmentWeight;
        public float CohesionWeight => _cohesionWeight;
        
        public Transform Center => _center;
        public Vector2 Size => _size;

        private IBoid[] _boids;
        
        private void Awake()
        {
            _boids = new IBoid[_count];
            for (var i = 0; i < _count; i++)
            {
                var go = Instantiate(_boidPrefab, transform);
                var boid = go.GetComponentInChildren<IBoid>();
                boid.Flock = this;
                boid.Spawn();
                _boids[i] = boid;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_center.position, _size);
        }
    }
}