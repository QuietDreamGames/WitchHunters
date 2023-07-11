using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.ColliderController.Core
{
    [Serializable]
    public class MeleeColliderInfo
    {
        // public Collider2D Collider;
        public Vector2 offset;
        public Vector2 size; 
        public MeleeColliderType meleeColliderType;
        
    }
}