using Unity.Collections;
using Unity.Entities;

namespace Features.DialogueHandler.Components
{
    public struct Dialogue : IComponentData
    {
        public bool Enable;

        public int CurrentPhraseId;
        public int AwaitedPhraseId;
    }
}