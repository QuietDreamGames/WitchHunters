using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.Scene.Components
{
    [BurstCompile]
    public struct PlayerSpawner : IComponentData
    {
        public float3 Center;
        public float2 Size;

        public PlayerSpawner(float3 center, float2 size)
        {
            Center = center;
            Size = size;
        }
    }
}