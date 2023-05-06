using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class FlockGO : MonoBehaviour
    {
        [SerializeField] private Boid _boidPrefab;
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

        private Boid[] _boids;
        
        public float MaxSpeed => _maxSpeed;
        
        public float SeparationWeight => _separationWeight;
        public float AlignmentWeight => _alignmentWeight;
        public float CohesionWeight => _cohesionWeight;
        
        public Transform Center => _center;
        public Vector2 Size => _size;

        private void Awake()
        {
            _boids = new Boid[_count];
            for (var i = 0; i < _count; i++)
            {
                var boid = Instantiate(_boidPrefab, transform);
                boid.Flock = this;
                boid.Position = GetPos();
                boid.Velocity = Vector3.zero;
                _boids[i] = boid;
            }
        }
        
        private Vector3 GetPos()
        {
            var centerPosition = _center.position;
            var posX = Random.Range(centerPosition.x - _size.x / 2, centerPosition.x + _size.x / 2);
            var posY = Random.Range(centerPosition.y - _size.y / 2, centerPosition.y + _size.y / 2);
            var modX = Mathf.Abs(math.fmod(posX, 2));
            var modY = Mathf.Abs(math.fmod(posY, 2));
            var signX = modX > 1f ? 1 : -1;
            var signY = modY > 1f ? 1 : -1;
            return new Vector3(posX * signX, posY * signY, 0);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_center.position, _size);
        }

        public (bool, Vector3) Separation(Boid self)
        {
            var steer = Vector3.zero;
            var total = 0;
            for (var i = 0; i < _boids.Length; i++)
            {
                var other = _boids[i];
                var dist = (other.Position - self.Position).magnitude;
                if (self != other && dist < _separationWeight)
                {
                    steer += other.Position;
                    total++;
                }
            }
            if (total > 0)
            {
                steer /= total;
                var dist = (steer - self.Position).magnitude;
                if (dist < _separationWeight)
                {
                    steer = -(steer - self.Position).normalized;
                }
            }
            return (total > 0, steer * _separationMultiplier);
        }
        
        public (bool, Vector3) Alignment(Boid self)
        {
            var steer = Vector3.zero;
            var total = 0;
            for (var i = 0; i < _boids.Length; i++)
            {
                var other = _boids[i];
                var dist = (other.Position - self.Position).magnitude;
                if (self != other && dist < _alignmentWeight)
                {
                    steer += other.Velocity;
                    total++;
                }
            }
            if (total > 0)
            {
                steer /= total;
                steer = steer.normalized;
            }
            return (total > 0, steer * _alignmentMultiplier);
        }
        
        public (bool, Vector3) Cohesion(Boid self)
        {
            var steer = Vector3.zero;
            var total = 0;
            for (var i = 0; i < _boids.Length; i++)
            {
                var other = _boids[i];
                var distance = (other.Position - self.Position).magnitude;
                if (self != other && distance < _cohesionWeight)
                {
                    steer += other.Position;
                    total++;
                }
                
            }
            if (total > 0)
            {
                steer /= total;
                steer = (steer - self.Position).normalized;
            }
            return (total > 0, steer * _cohesionMultiplier);
        }

        public (bool, Vector3) Bounds(Boid self)
        {
            var centerPosition = _center.position;
            var steer = Vector3.zero;
            var outOfBounds = false;
            if (self.Position.x > (centerPosition.x + _size.x / 2))
            {
                steer.x = -_maxSpeed;
                outOfBounds = true;
            }
            else if (self.Position.x < centerPosition.x - _size.x / 2)
            {
                steer.x = _maxSpeed;
                outOfBounds = true;
            }
            if (self.Position.y > centerPosition.y + _size.y / 2)
            {
                steer.y = -_maxSpeed;
                outOfBounds = true;
            }
            else if (self.Position.y < centerPosition.y - _size.y / 2)
            {
                steer.y = _maxSpeed;
                outOfBounds = true;
            }
            return (outOfBounds, steer);
        }
    }
}
