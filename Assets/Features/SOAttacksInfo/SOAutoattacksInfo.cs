using Features.Character.Services;
using UnityEngine;

namespace Features.SOAttacksInfo
{
    [CreateAssetMenu(fileName = "AutoattacksInfo", menuName = "Attacks/AutoattacksInfo")]
    public class SOAutoattacksInfo : ScriptableObject
    {
        public AutoattackInfo[] AutoattackInfos;
    }
}