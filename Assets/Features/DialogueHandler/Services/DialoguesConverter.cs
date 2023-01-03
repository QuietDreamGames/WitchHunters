using Features.DialogueHandler.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Features.DialogueHandler.Services
{
    public class DialoguesConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data


        [SerializeField] private SODialoguesData _soDialoguesData;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var dialogue = new Dialogue
            {
                Enable = false
            };
            dstManager.AddComponentData(entity, dialogue);

            var dialoguesData = new DialoguesData()
            {
                Value = CreateDialoguesData()
            };

            dstManager.AddComponentData(entity, dialoguesData);
        }

        private BlobAssetReference<DialoguesDataPool> CreateDialoguesData()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref DialoguesDataPool dialoguesData = ref builder.ConstructRoot<DialoguesDataPool>();

            var numDialogues = _soDialoguesData.DialogueDatas.Length;

            BlobBuilderArray<DialogueData> arrayBuilder = builder.Allocate(
                ref dialoguesData.DialoguesData,
                numDialogues
            );

            // Initialize the dialogues.
            for (var i = 0; i < _soDialoguesData.DialogueDatas.Length; i++)
            {
                arrayBuilder[i].PhraseKey = _soDialoguesData.DialogueDatas[i].PhraseKey;
                arrayBuilder[i].SpeakerCharacter = (int) _soDialoguesData.DialogueDatas[i].SpeakerCharacter;
            }

            var result = builder.CreateBlobAssetReference<DialoguesDataPool>(Allocator.Persistent);
            builder.Dispose();
            return result;
        }
        
        #endregion
    }
}