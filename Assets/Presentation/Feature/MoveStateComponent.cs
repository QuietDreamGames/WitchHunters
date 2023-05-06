using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MoveStateComponent : IComponentData
{ 
    public float3 TargetPos;
    public float3 Direction;
}
