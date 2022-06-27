using System;

namespace Features.Collision.Utils
{
    [Flags]
    public enum CollisionLayers
    {
        Ground = 1 << 0,
        Player = 1 << 1,
        Enemy = 1 << 2,
    }
}
