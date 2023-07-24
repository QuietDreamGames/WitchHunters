using System;

namespace Features.VFX.Core
{
    [Serializable]
    public enum HitEffectType
    {
        Physical = 0,
        PhysicalMelee = 1,
        PhysicalRanged = 2,
        PhysicalAOE = 3,
        Flame = 4,
        FlameMelee = 5,
        FlameRanged = 6,
        FlameAOE = 7,
    }
}