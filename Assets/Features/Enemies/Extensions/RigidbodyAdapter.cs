using FishNet.Object;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class RigidbodyAdapter : NetworkBehaviour
    {
        [SerializeField] private new Rigidbody2D rigidbody2D;

        public bool Active
        {
            get => rigidbody2D.bodyType == RigidbodyType2D.Dynamic;
            set
            {
                if (!IsOwner)
                {
                    return;
                }
                
                rigidbody2D.bodyType = value
                    ? RigidbodyType2D.Dynamic
                    : RigidbodyType2D.Static;
            }
        }

        public Vector2 Origin
        {
            get => rigidbody2D.position;
            set => rigidbody2D.position = value;
        }
        
        public Vector2 Velocity
        {
            get => rigidbody2D.velocity;
            set => rigidbody2D.velocity = value;
        }

        public override void OnStartClient()
        {
            if (!IsOwner)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }

        public void SetActiveRigidbody()
        {
            Active = true;
        }
        
        public void SetUnactiveRigidbody()
        {
            Active = false;
        }
    }
}
