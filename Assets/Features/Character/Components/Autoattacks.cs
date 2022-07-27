using Features.Character.Services;
using Unity.Entities;

namespace Features.Character.Components
{
    public struct Autoattacks : IComponentData
    {
        public BlobAssetReference<AutoattackInfoPool> Value;
    }
}