using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeReference] private GameObject _mover;
        [SerializeField] private int _count;
    
        [SerializeField] private Transform _center;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2 _waitRange;
        [SerializeField] private float _speed;

        private void Start()
        {
            for (var i = 0; i < _count; i++)
            {
                var go = Instantiate(_mover, transform.position, Quaternion.identity);
                var mover = go.GetComponent<IMover>();
                if (mover == null)
                {
                    mover = go.GetComponentInChildren<IMover>();
                }
            
                mover.Center = _center;
                mover.Size = _size;
                mover.WaitRange = _waitRange;
                mover.Speed = _speed;
                
                mover.Spawn();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_center.position, _size);
        }
    }
}
