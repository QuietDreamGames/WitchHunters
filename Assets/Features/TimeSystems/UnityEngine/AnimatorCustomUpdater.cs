using Features.TimeSystems.Interfaces.Handlers;
using UnityEngine;

namespace Features.TimeSystems.UnityEngine
{
    public class AnimatorCustomUpdater : MonoBehaviour, IUpdateHandler
    {
        [SerializeField] private Animator animator;
        
        public void OnUpdate(float deltaTime)
        {
            animator.Update(deltaTime);
        }
    }
}