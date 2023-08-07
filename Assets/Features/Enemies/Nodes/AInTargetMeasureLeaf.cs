using Features.BTrees.Core;
using UnityEngine;

namespace Features.Enemies.Nodes
{
    public abstract class AInTargetMeasureLeaf : LeafNode
    {
        public abstract Vector3 GetClosestPoint(Vector3 origin);
    }
}
