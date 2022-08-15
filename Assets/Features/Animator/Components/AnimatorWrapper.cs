using System;
using Unity.Entities;

namespace Features.Animator.Components
{
    public class AnimatorWrapper : IComponentData
    {
        public UnityEngine.Animator Value;
    }
}
