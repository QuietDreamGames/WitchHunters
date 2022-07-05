using Features.StateMachine.Components.Core;
using Unity.Entities;
using UnityEngine;

namespace Features.StateMachine.Components.Nodes.Decorator
{
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
