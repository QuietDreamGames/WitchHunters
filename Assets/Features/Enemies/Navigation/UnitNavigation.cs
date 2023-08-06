using System;
using Features.TimeSystems.Interfaces.Handlers;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

namespace Features.Enemies.Navigation
{
    public class UnitNavigation : MonoBehaviour, IFixedUpdateHandler
    {
        [SerializeField] private float tickRate = 20;
        [SerializeField] private float nextWaypointDistance = .25f;


        [Header("Dependencies")]
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private Seeker seeker;

        private NavigationComputer _navigationComputer;

        private Vector2 _target;

        private Path _path;
        private int _currentWaypoint;
        
        private bool _isActive;

        private void Awake()
        {
            _navigationComputer = new NavigationComputer(tickRate, UpdatePath);
        }

        public void SetActive(bool active)
        {
            _isActive = active;
            if (_isActive)
            {
                _navigationComputer.Reset();
            }
        }

        public void SetTarget(Vector2 target)
        {
            _target = target;
        }

        public bool TryGetDirection(out Vector2 direction)
        {
            direction = Vector2.zero;
            
            if (_path == null)
            {
                return false;
            }

            bool inDistance;
            Vector2 origin;
            Vector2 waypoint;

            do
            {
                if (_currentWaypoint >= _path.vectorPath.Count)
                {
                    return false;
                }

                origin = rigidbody2D.position;
                waypoint = _path.vectorPath[_currentWaypoint];

                direction = (waypoint - origin).normalized;
                var distance = Vector2.Distance(waypoint, origin);

                inDistance = distance < nextWaypointDistance;
                if (inDistance)
                {
                    _currentWaypoint++;
                }
            } while (inDistance);
            
            return true;
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }
            
            _navigationComputer.Update(deltaTime);
        }

        private void UpdatePath()
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rigidbody2D.position, _target, OnPathHandler);
            }
        }
        
        private void OnPathHandler(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (rigidbody2D != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(rigidbody2D.position, nextWaypointDistance);
            }
        }

        private struct NavigationComputer
        {
            private const int TickCoef = 1000;

            private readonly Action _onUpdate;

            private readonly int _ticks;
            private int _currentTick;

            public NavigationComputer(float tickRate, Action onUpdate)
            {
                _onUpdate = onUpdate;
                
                _ticks = (int)((tickRate / 60f) * TickCoef);

                _currentTick = 0;
            }

            public void Update(float deltaTime)
            {
                var deltaTicks = (int)(deltaTime * TickCoef);
                _currentTick += deltaTicks;
                if (_currentTick >= _ticks)
                {
                    _onUpdate?.Invoke();
                    _currentTick %= _ticks;
                }
            }

            public void Reset()
            {
                _onUpdate?.Invoke();
                _currentTick = 0;
            }
        }
    }
}
