using Unity.Entities;

namespace Features.Character.Services
{
    public struct AutoattackInfoPool
    {
        public BlobArray<AutoattackInfo> AutoattackInfos;
    }
}