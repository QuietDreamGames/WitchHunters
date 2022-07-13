using System;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using LogType = Features.BehaviourTree.Components.Nodes.Leaf.LogType;

namespace Features.BehaviourTree.Services.Core
{
    public static class JobLogger
    {
        [BurstDiscard] 
        public static void Log(FixedString32Bytes message, LogType type)
        {
            switch (type)
            {
                case LogType.Simple:
                    Debug.Log(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}