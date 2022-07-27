using Features.Character.Components;
using Features.SOAttacksInfo;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Features.Character.Services
{
    public class AutoattackConverter : MonoBehaviour, IConvertGameObjectToEntity
    {
        #region Serializable data


        [SerializeField] private SOAutoattacksInfo _soAutoattacksInfo;

        #endregion

        #region IConvertGameObjectToEntity implementation

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var attack = new Attack
            {
                Enable = false,
                CurrentAttackId = 0
            };
            dstManager.AddComponentData(entity, attack);

            var autoattacks = new Autoattacks
            {
                Value = CreateAutoattackInfos()
            };

            dstManager.AddComponentData(entity, autoattacks);


        }

        private BlobAssetReference<AutoattackInfoPool> CreateAutoattackInfos()
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref AutoattackInfoPool autoattackInfos = ref builder.ConstructRoot<AutoattackInfoPool>();

            var numAutoattacks = _soAutoattacksInfo.AutoattackInfos.Length;

            BlobBuilderArray<AutoattackInfo> arrayBuilder = builder.Allocate(
                ref autoattackInfos.AutoattackInfos,
                numAutoattacks
            );

            // Initialize the autoattacks.

            foreach (var autoattackInfo in _soAutoattacksInfo.AutoattackInfos)
            {
                arrayBuilder[autoattackInfo.Id] = autoattackInfo;
            }

            var result = builder.CreateBlobAssetReference<AutoattackInfoPool>(Allocator.Persistent);
            builder.Dispose();
            return result;
        }
        
        #endregion

    }



    
}
    
