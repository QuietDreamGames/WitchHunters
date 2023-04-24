using Unity.Entities;
using UnityEngine;

namespace Features.UI.DeathScreen.Components
{
    public class DeathScreen : IComponentData
    {
        public GameObject GameObject;
        public UnityEngine.Animator Animator;
        public bool IsRunning;
    }
}