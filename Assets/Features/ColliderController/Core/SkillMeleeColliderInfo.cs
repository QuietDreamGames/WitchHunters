using System;
using UnityEngine;

namespace Features.ColliderController.Core
{
    [Serializable]
    public class SkillMeleeColliderInfo
    {
        // public Collider2D Collider;
        public Vector2 offset;
        public Vector2 size;
        public SkillMeleeColliderType meleeColliderType;

    }
}