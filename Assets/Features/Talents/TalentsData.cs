using System;
using System.Collections.Generic;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using UnityEngine;

namespace Features.Talents
{
    [Serializable]
    public class TalentsData
    {
        public int talentPoints;
        public List<TalentData> learnedTalents;
    }

    [Serializable]
    public class TalentData
    {
        public string talentName;
        public TalendDataEntry[] talentData;
        public Sprite icon;

    }

    [Serializable]
    public class TalendDataEntry
    {
        
        public ModifierType modifierType;
        public ModifierData modifier;
        
    }
}