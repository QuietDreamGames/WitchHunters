﻿using Unity.Entities;

namespace Features.BehaviourTree.Systems.Core.SystemGroups
{
    [UpdateInGroup(typeof(StateMachineSystemGroup), OrderLast = true)]
    public partial class LeafNodeSystemGroup : ComponentSystemGroup
    {
        
    }
}