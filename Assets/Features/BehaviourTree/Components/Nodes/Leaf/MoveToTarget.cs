using Features.BehaviourTree.Components.Core;
using Features.BehaviourTree.Services.Geometry;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Features.BehaviourTree.Components.Nodes.Leaf
{
    [BurstCompile]
    public readonly struct MoveToTarget : IComponentData, INode
    {
        public readonly float Speed;
        
        public readonly float AcceptableRadius;
        public readonly RectPolygon Polygon;

        public readonly RadiusType Type;
        
        #region Constructor

        public MoveToTarget(float acceptableRadius, float speed) : this()
        {
            AcceptableRadius = acceptableRadius;
            Speed = speed;

            Type = RadiusType.Circle;
        }

        public MoveToTarget(float2 zone, float2 excludeZone, float speed) : this()
        {
            Polygon = new RectPolygon(zone, excludeZone);
            Speed = speed;

            Type = RadiusType.RectPolygon;
        }

        #endregion

        #region Public methods

        public (bool, float3) ComputePath(float3 self, float3 target)
        {
            switch (Type)
            {
                case RadiusType.Circle:
                {
                    var distance = math.distance(target, self);
                    var direction = math.normalize(target - self);

                    var completed = distance < AcceptableRadius;
                
                    return (completed, direction);
                }
                case RadiusType.RectPolygon:
                {
                    var relativeSelf = self - target;
                    var closestPoint = Polygon.ClosestPoint(relativeSelf);
                
                    var distance = math.distance(closestPoint, relativeSelf);
                    var direction = math.normalize(closestPoint - relativeSelf);

                    const float error = 0.05f;
                    var completed = distance < error;

                    return (completed, direction);
                }
                default:
                    return (true, float3.zero);
            }
        }

        #endregion

        public enum RadiusType
        {
            Circle = 0,
            RectPolygon
        }
    }
}