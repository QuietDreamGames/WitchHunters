using Features.Character.Components;
using Features.StateMachine.Components;
using Features.StateMachine.Components.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Features.StateMachine.Services
{
    public class EnemyBehaviourTestConvert : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var movement = new Movement
            {
                Direction = float3.zero,
                Enable = false
            };
            dstManager.AddComponentData(entity, movement);

            var speed = new Speed
            {
                Value = 1,
            };
            dstManager.AddComponentData(entity, speed);
            
            int actionCount = 0;

            var sEntity = dstManager.CreateEntity(typeof(NodeComponent), typeof(Sequence));
         
            PrepareSequenceAction(ref dstManager, in entity, in sEntity, actionCount, new MoveDirection(new float3(-1, 0, 0), 5));
            ++actionCount;
         
            PrepareSequenceAction(ref dstManager, in entity, in sEntity, actionCount, new MoveDirection(new float3(1, 0, 0), 5));
            ++actionCount;
            
            dstManager.AddComponentData(sEntity, new Sequence(sEntity, actionCount));
            dstManager.AddComponentData(sEntity, new NodeComponent(entity, entity, 0) {IsExec = true});
        }

        private static void PrepareSequenceAction<T>(ref EntityManager dstManager, 
            in Entity rootEntity, in Entity agentEntity, int actionIndex, T actionFilter) 
            where T : struct, INodeComponent
        {
            var node = dstManager.CreateEntity(typeof(NodeComponent), typeof(T));
            dstManager.AddComponentData(node, new NodeComponent(rootEntity, agentEntity, actionIndex));
            dstManager.AddComponentData(node, actionFilter);
        }
    }
}