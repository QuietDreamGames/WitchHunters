using System;
using UnityEngine;

namespace Features.ColliderController.Core
{
    [Serializable]
    public class ColliderInfo
    {
        // public Collider2D Collider;
        public Vector2 offset;
        public Vector2 size; 
        public ColliderType ColliderType;
        
    }
    
    [Serializable]
    public enum ColliderType
    {
        U1 = 0,
        U2 = 1,
        U3 = 2,
        R1 = 3,
        R2 = 4,
        R3 = 5,
        D1 = 6,
        D2 = 7,
        D3 = 8,
        L1 = 9,
        L2 = 10,
        L3 = 11,
    }
}