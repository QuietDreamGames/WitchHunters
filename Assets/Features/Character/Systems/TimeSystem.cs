using Features.Character.Components;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Systems
{
    public partial class TimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new DeltaTimeJob
            {
                DeltaTime = Time.DeltaTime,
            }.ScheduleParallel();
        }
    }
    
    public partial struct DeltaTimeJob : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(ref DeltaTime deltaTime)
        {
            deltaTime.Value = DeltaTime;
        }
    }
}