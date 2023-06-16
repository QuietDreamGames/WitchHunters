using System;

namespace Features.ColliderController.Core
{
    [Serializable]
    public class RangeColliderInfo
    {
        public RangeColliderType rangeColliderType;
        public int baseAmountInBurst = 3;
        public int baseBurstTimes = 1;
        public float baseBurstInterval = 0.5f;
        public float additionalAngle = 25f;
    }
}