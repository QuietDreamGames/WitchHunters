using System;

namespace Features.Character.Services
{
    [Serializable]
    public struct AutoattackInfo
    {
        public int Id;
        public float Time;
        public float BaseDamage;
    }
}