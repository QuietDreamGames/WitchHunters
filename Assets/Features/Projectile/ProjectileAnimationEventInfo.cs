using Features.ColliderController.Core;
using UnityEngine;

namespace Features.Projectile
{
    [CreateAssetMenu(fileName = "ProjectileAnimationEventInfo", menuName = "Projectile/ProjectileAnimationEventInfo")]
    public class ProjectileAnimationEventInfo : ScriptableObject
    {
        public RangeColliderType rangeColliderType;
        public Vector3 offset;
        public Vector3 direction;
        public float angle;
        
    }
}