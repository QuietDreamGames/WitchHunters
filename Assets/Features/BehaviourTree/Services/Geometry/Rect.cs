using Unity.Burst;
using Unity.Mathematics;

namespace Features.BehaviourTree.Services.Geometry
{
    [BurstCompile]
    public readonly struct Rect
    {
        public readonly float HorizontalMin;
        public readonly float HorizontalMax;

        public readonly float VerticalMin;
        public readonly float VerticalMax;

        #region Constructor

        public Rect(float2 zone)
        {
            var horizontal = zone.x / 2;
            var vertical = zone.y / 2;
            
            HorizontalMin = -horizontal;
            HorizontalMax = horizontal;

            VerticalMin = -vertical;
            VerticalMax = vertical;
        }

        public Rect(float2 zone, Rect limiter)
        {
            var horizontal = zone.x / 2;
            var vertical = zone.y / 2;
            
            HorizontalMin = math.clamp(-horizontal, limiter.HorizontalMin, limiter.HorizontalMax);
            HorizontalMax = math.clamp(horizontal, limiter.HorizontalMin, limiter.HorizontalMax);

            VerticalMin = math.clamp(-vertical, limiter.VerticalMin, limiter.VerticalMax);
            VerticalMax = math.clamp(vertical, limiter.VerticalMin, limiter.VerticalMax);
        }

        public Rect(float horizontalMin, float horizontalMax, float verticalMin, float verticalMax)
        {
            HorizontalMin = horizontalMin;
            HorizontalMax = horizontalMax;

            VerticalMin = verticalMin;
            VerticalMax = verticalMax;
        }

        #endregion

        #region Public methods

        public float Area()
        {
            var x = HorizontalMax - HorizontalMin;
            var y = VerticalMax - VerticalMin;
            return x * y;
        }

        public bool IsInside(float2 point)
        {
            if (point.x < HorizontalMin || point.x > HorizontalMax)
                return false;

            if (point.y < VerticalMin || point.y > VerticalMax)
                return false;

            return true;
        }

        #endregion
    }
}