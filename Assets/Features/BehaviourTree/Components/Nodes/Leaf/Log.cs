using Features.BehaviourTree.Components.Core;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public struct Log : IComponentData, INode
    {
        public readonly FixedString32Bytes Message;

        public readonly LogType Type;

        public Log(string message, LogType type) : this()
        {
            Message = message;
            Type = type;
        }
    }

    public enum LogType
    {
        Simple = 0,
        Warning,
        Error
    }
}