using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class MoveGO : MonoBehaviour, IMover
    {
        #region Public properties

        public Transform Center { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 WaitRange { get; set; }
        public float Speed { get; set; }

        #endregion

        #region Private fields

        [SerializeField] private Animator _animator;
        
        [SerializeField] private string _horizontal;
        [SerializeField] private string _vertical;
        [SerializeField] private string _moving;

        #endregion

        #region Private fields

        private Vector3 _targetPos;
        private Vector3 _direction;

        private bool _isMoving;

        #endregion

        public void Spawn()
        {
            transform.position = GetPos();

            _isMoving = false;
        }
    
        private void Update()
        {
            _animator.SetFloat(_horizontal, _direction.x); 
            _animator.SetFloat(_vertical, _direction.y);
            _animator.SetBool(_moving, _isMoving);
            
            if (_isMoving)
            {
                var pos = transform.position;
                var dist =  Vector3.Distance(pos, _targetPos);
                if (dist < 0.1f)
                {
                    _isMoving = false;
                    return;
                }
            
                _direction = (_targetPos - pos).normalized;
                var diff = _direction * Speed * Time.deltaTime;
                transform.position += diff;
            }
            else
            {
                _targetPos = GetPos();
                _isMoving = true;
            }
        }

        private Vector3 GetPos()
        {
            var centerPosition = Center.position;
            var posX = Random.Range(centerPosition.x - Size.x / 2, centerPosition.x + Size.x / 2);
            var posY = Random.Range(centerPosition.y - Size.y / 2, centerPosition.y + Size.y / 2);
            var modX = Mathf.Abs(math.fmod(posX, 2));
            var modY = Mathf.Abs(math.fmod(posY, 2));
            var signX = modX > 1f ? 1 : -1;
            var signY = modY > 1f ? 1 : -1;
            return new Vector3(posX * signX, posY * signY, 0);
        }
    }
}
