using Features.Animator.Systems;
using Features.Character.Components;
using Features.Character.Systems.SystemGroups;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Systems
{
    [UpdateInGroup(typeof(GameObjectSyncGroup))]
    public partial class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new MoveJob
            {
                DeltaTime = Time.fixedDeltaTime
            }.Run();
        }
    }
    
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(Rigidbody2DWrapper rigidbody2D, in Movement movement, in Speed speed)
        {
            if (movement.Enable)
            {
                var velocity = movement.Direction * speed.Value * DeltaTime;
                rigidbody2D.Value.velocity = new Vector2(velocity.x, velocity.y);
            }
            else
            {
                rigidbody2D.Value.velocity = Vector2.zero;
            }
        }
    }
}