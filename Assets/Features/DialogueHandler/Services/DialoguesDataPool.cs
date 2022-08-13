using Unity.Entities;
using Unity.Physics;

namespace Features.DialogueHandler.Services
{
    public struct DialoguesDataPool
    {
        public BlobArray<DialogueData> Value;
    }
}