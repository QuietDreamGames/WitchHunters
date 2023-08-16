using System;
using UnityEngine;

namespace Features.Talents
{
    [CreateAssetMenu(fileName = "TalentsList", menuName = "Data/TalentsList")]
    public class TalentsList : ScriptableObject
    {
        public TalentData[] talents;
    }
}