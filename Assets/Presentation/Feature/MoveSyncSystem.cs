using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

namespace Presentation.Feature
{
    [BurstCompile]
    public partial class MoveSyncSystem : SystemBase
    {
        private EntityQuery _query;
        
        private NativeArray<Translation> _translations;
        private TransformAccessArray _transformAccessArray;

        protected override void OnCreate()
        {
            base.OnCreate();
            _query = GetEntityQuery(
                typeof(TransformComponent),
                typeof(Translation));
        }

        protected override void OnUpdate()
        {
            if (_translations.IsCreated)
            {
                _translations.Dispose();
            }
            
            if (_transformAccessArray.isCreated)
            {
                _transformAccessArray.Dispose();
            }
            
            _translations = _query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var transformComponents = _query.ToComponentDataArray<TransformComponent>();
            var transforms = new Transform[transformComponents.Length];
            
            for (var i = 0; i < _translations.Length; i++)
            {
                transforms[i] = transformComponents[i].Value;
            }

            _transformAccessArray = new TransformAccessArray(transforms);
            
            var moveSyncJob = new MoveSyncJob
            {
                Translations = _translations,
            };
            
            Dependency = moveSyncJob.Schedule(_transformAccessArray, Dependency);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_translations.IsCreated)
            {
                _translations.Dispose();
            }
            
            if (_transformAccessArray.isCreated)
            {
                _transformAccessArray.Dispose();
            }
        }
        
        [BurstCompile]
        private struct MoveSyncJob : IJobParallelForTransform
        {
            [ReadOnly] 
            public NativeArray<Translation> Translations;
            
            public void Execute(int index, TransformAccess transform)
            {
                transform.position = Translations[index].Value;
            }
        }
    }
}
