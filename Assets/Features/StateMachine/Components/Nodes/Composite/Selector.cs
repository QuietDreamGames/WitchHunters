﻿using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Components.Nodes.Composite
{
    public struct Selector : IComponentData, INode
    {
        public readonly int NodeCount;
		
        public int CurrentNodeIndex;

        public Selector(int nodeCount) : this()
        {
            NodeCount = nodeCount;
        }
    }
}