using System;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class CollisionFacade : MonoBehaviour
    {
        public Action<Collision2D> OnCollision2DEnter;
        public Action<Collision2D> OnCollision2DStay;
        public Action<Collision2D> OnCollision2DExit;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollision2DEnter?.Invoke(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            OnCollision2DStay?.Invoke(other);
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            OnCollision2DExit?.Invoke(other);
        }
    }
}
