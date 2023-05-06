using Features.Animator.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Presentation.Feature
{
    public class FlockConverter : MonoBehaviour, IBoid, IConvertGameObjectToEntity
    {
        public FlockEntitySpawner Flock { get; set; }
        
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

            gameObject.AddComponent<ConvertToEntity>();
        }
        
        private Vector3 GetPos()
        {
            var centerPosition = Flock.Center.position;
            var posX = Random.Range(centerPosition.x - Flock.Size.x / 2, centerPosition.x + Flock.Size.x / 2);
            var posY = Random.Range(centerPosition.y - Flock.Size.y / 2, centerPosition.y + Flock.Size.y / 2);
            return new Vector3(posX, posY, 0);
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new FlockComponent
            {
                Velocity = float3.zero,
                MaxSpeed = Flock.MaxSpeed,
            });

            dstManager.AddComponentData(entity, new SeparationComponent
            {
                Enabled = false,
                Value = float3.zero,

                Weight = Flock.SeparationWeight,
            });
            
            dstManager.AddComponentData(entity, new AlignmentComponent
            {
                Enabled = false,
                Value = float3.zero,

                Weight = Flock.AlignmentWeight,
            });
            
            dstManager.AddComponentData(entity, new CohesionComponent
            {
                Enabled = false,
                Value = float3.zero,

                Weight = Flock.CohesionWeight,
            });
            
            dstManager.AddComponentData(entity, new BoundsComponent
            {
                Enabled = false,
                Value = float3.zero,

                Size = Flock.Size,
                Center = Flock.Center.position,
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