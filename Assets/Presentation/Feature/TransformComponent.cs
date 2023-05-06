using Unity.Entities;
using UnityEngine;

namespace Presentation.Feature
{
    public class TransformComponent : IComponentData
    {
        public Transform Value;
    }
}
