using Features.DialogueHandler.Services;
using Unity.Entities;

namespace Features.DialogueHandler.Components
{
    public struct DialoguesData : IComponentData
    {
        public BlobAssetReference<DialoguesDataPool> Value;
    }
}