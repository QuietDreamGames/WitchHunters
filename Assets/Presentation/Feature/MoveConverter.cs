using Features.Animator.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class MoveConverter : MonoBehaviour, IMover ,IConvertGameObjectToEntity
    {
        public Transform Center { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 WaitRange { get; set; }
        public float Speed { get; set; }
        
        #region Private fields

        [SerializeField] private Transform _origin;
        [SerializeField] private Animator _animator;
        
        [SerializeField] private string _horizontal;
        [SerializeField] private string _vertical;
        [SerializeField] private string _moving;

        #endregion
    
        public void Spawn()
        {
            _origin.position = GetPos();
            
            var stopComponent = GetComponent<StopConvertToEntity>();
            Destroy(stopComponent);
        }
        
        private Vector3 GetPos()
        {
            var centerPosition = Center.position;
            var posX = Random.Range(centerPosition.x - Size.x / 2, centerPosition.x + Size.x / 2);
            var posY = Random.Range(centerPosition.y - Size.y / 2, centerPosition.y + Size.y / 2);
            return new Vector3(posX, posY, 0);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MoveComponent
            {
                Center = new float2(Center.position.x, Center.position.y),
                Size = Size,
                WaitRange = WaitRange,
                Speed = Speed,
                
                ClampMin = new float3(Center.position.x - Size.x / 2, Center.position.y - Size.y / 2, 0),
                ClampMax = new float3(Center.position.x + Size.x / 2, Center.position.y + Size.y / 2, 0),
            });
            
            dstManager.AddComponentData(entity, new MoveStateComponent
            {
                TargetPos = GetPos(),
                Direction = float3.zero,
            });

            dstManager.AddComponentData(entity, new AnimatorWrapper
            {
                Value = _animator,
            });
            
            dstManager.AddComponentData(entity, new AnimatorConfComponent
            {
                Horizontal = _horizontal,
                Vertical = _vertical,
                Moving = _moving,
            });

            dstManager.AddComponentData(entity, new TransformComponent
            {
                Value = _origin,
            });

            dstManager.AddComponentData(entity, new Translation
            {
                Value = _origin.position,
            });
        }
    }
}
