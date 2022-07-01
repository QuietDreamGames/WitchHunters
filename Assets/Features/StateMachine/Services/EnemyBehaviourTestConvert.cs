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
            
            int actionCount = 0;
         
            PrepareSequenceAction(ref dstManager, in entity, actionCount, new MoveDirection(new float3(-1, 0, 0), 5));
            ++actionCount;
         
            PrepareSequenceAction(ref dstManager, in entity, actionCount, new MoveDirection(new float3(1, 0, 0), 5));
            ++actionCount;
            
            dstManager.AddComponentData(entity, new NodeAgent(actionCount));
        }

        private static void PrepareSequenceAction<T>(ref EntityManager dstManager, 
            in Entity entity, int actionIndex, T actionFilter) 
            where T : struct, INodeComponent
        {
            var node = dstManager.CreateEntity(typeof(NodeComponent), typeof(T));
            dstManager.AddComponentData(node, new NodeComponent(entity, actionIndex));
            dstManager.AddComponentData(node, actionFilter);
        }
    }
}