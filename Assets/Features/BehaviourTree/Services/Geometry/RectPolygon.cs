using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Features.BehaviourTree.Services.Geometry
{
    [BurstCompile]
    public readonly struct RectPolygon
    {
        private readonly FixedList128Bytes<Rect> _rects;

        #region Constructor

        public RectPolygon(float2 zone, float2 excludeZone) : this()
        {
            var rect = new Rect(zone);
            var excludeRect = new Rect(excludeZone, rect);

            var rects = ConstructRects(rect, excludeRect);
            _rects = new FixedList128Bytes<Rect>();

            for (var i = 0; i < rects.Length; i++)
            {
                var el = rects[i];
                if (el.Area() == 0)
                {
                    continue;
                }

                _rects.Add(el);
            }
        }

        #endregion

        #region Public methods

        public readonly float3 ClosestPoint(float3 position)
        {
            var closestPoint = float3.zero;
            var minDistance = math.INFINITY;

            for (var i = 0; i < _rects.Length; i++)
            {
                var rect = _rects[i];

                if (rect.IsInside(position.xy))
                {
                    return position;
                }

                var point = float3.zero;
                
                point.x = math.clamp(position.x, rect.HorizontalMin, rect.HorizontalMax);
                point.y = math.clamp(position.y, rect.VerticalMin, rect.VerticalMax);

                var distance = math.distance(position, point);

                if (distance < minDistance)
                {
                    closestPoint = point;
                    minDistance = distance;
                }
            }

            return closestPoint;
        }

        #endregion
        
        #region Private methods

        private Rect[] ConstructRects(Rect maxRect, Rect minRect)
        {
            return new[]
            {
                new Rect(minRect.HorizontalMin,
                    minRect.HorizontalMax,
                    minRect.VerticalMax,
                    maxRect.VerticalMax),
                new Rect(minRect.HorizontalMin,
                    minRect.HorizontalMax,
                    maxRect.VerticalMin,
                    minRect.VerticalMin),
                new Rect(maxRect.HorizontalMin,
                    minRect.HorizontalMin,
                    maxRect.VerticalMin,
                    maxRect.VerticalMax),
                new Rect(minRect.HorizontalMax,
                    maxRect.HorizontalMax,
                    maxRect.VerticalMin,
                    maxRect.VerticalMax),
            };
        }

        #endregion
    }
}