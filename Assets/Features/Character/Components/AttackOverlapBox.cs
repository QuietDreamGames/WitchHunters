using Unity.Burst;
using Unity.Entities;

namespace Features.Character.Components
{
    [BurstCompile]
    public struct AttackOverlapBox : IComponentData
    {
        public bool Enable;
        
    }
}