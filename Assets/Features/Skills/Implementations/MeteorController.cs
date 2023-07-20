// using System;
// using System.Collections;
// using UnityEngine;
//
// namespace Features.Skills.Implementations
// {
//     public class MeteorController : MonoBehaviour
//     {
//         [SerializeField] private Vector3 _startDirection;
//         [SerializeField] private float _distance;
//         [SerializeField] private float _timeBeforeHit;
//         [SerializeField] private float _timeAfterHit;
//         [SerializeField] private float _startTime;
//         
//         
//         
//         private bool _isUpdating;
//         private bool _isFalling;
//         private float _timer;
//         
//         private IEnumerator Start()
//         {
//             yield return new WaitForSeconds(_startTime);
//             Cast(Vector3.zero);
//         }
//         
//         public void Cast(Vector3 direction)
//         {
//             _meteorTransform.parent = null;
//             _meteorTransform.gameObject.SetActive(true);
//             _meteorTransform.position = transform.position + _startDirection.normalized * _distance;
//             _meteorTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_startDirection.y, _startDirection.x) * Mathf.Rad2Deg - 90);
//             _isUpdating = true;
//             _isFalling = true;
//             _meteorExplosionTransform.gameObject.SetActive(false);
//             _meteorExplosionCraterTransform.gameObject.SetActive(false);
//             _timer = _timeBeforeHit;
//         }
//         
//         public void OnUpdate(float deltaTime)
//         {
//             
//         }
//
//         public void OnFixedUpdate(float deltaTime)
//         {
//             
//         }
//
//         private void Update()
//         {
//             if (!_isUpdating) return;
//             _timer -= Time.deltaTime;
//             switch (_timer)
//             {
//                 case <= 0 when _isFalling:
//                     _isFalling = false;
//                     _timer = _timeAfterHit;
//                     StartCoroutine(Restart());
//                     break;
//                 case <= 0:
//                     _isUpdating = false;
//                     _timer = float.PositiveInfinity;
//                     break;
//             }
//         }
//
//         private void FixedUpdate()
//         {
//             if (!_isUpdating) return;
//             _meteorTransform.position = Vector3.MoveTowards(_meteorTransform.position, transform.position,
//                 _distance / _timeBeforeHit * Time.fixedDeltaTime);
//         }
//         
//         private IEnumerator Restart()
//         {
//             yield return new WaitForSeconds(5f);
//             Cast(Vector3.zero);
//         }
//         
//         
//         
//     }
// }