using Unity.Entities;
using UnityEngine;

namespace Features.UI.RegularEnemyHealth.Components
{
    public class RegularHealthBar : IComponentData
    {
        public Transform FullHealthBarTransform;
        public Transform CurrentHealthBarTransform;
    }
}