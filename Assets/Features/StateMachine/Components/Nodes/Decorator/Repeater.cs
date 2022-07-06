using Features.StateMachine.Components.Core;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Features.StateMachine.Components.Nodes.Decorator
{
    [BurstCompile]
    public struct Repeater : IComponentData, INode
    {
        public RepeaterType Type;

        public Repeater(RepeaterType type) : this()
        {
            Type = type;
        }
    }

    public enum RepeaterType
    {
        Forever,
        Success,
        Failed
    }
}
