using Features.DialogueHandler.Components;
using Unity.Entities;

namespace Features.DialogueHandler.Systems
{
    public partial class DialogueSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            new StartDialogueJob{}.Run();
        }
    }

    public partial struct StartDialogueJob : IJobEntity
    {
        public void Execute(ref Dialogue dialogue, in DialoguesData dialoguesData)
        {
            if (!dialogue.Enable) return;

            if (dialogue.AwaitedPhraseId == dialogue.CurrentPhraseId) return;

            dialogue.CurrentPhraseId = dialogue.AwaitedPhraseId;
            
        }
    }
}