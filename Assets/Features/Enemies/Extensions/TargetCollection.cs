using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Enemies.Extensions
{
    public class TargetCollection : MonoBehaviour
    {
        private readonly SortedSet<TargetData> targets = new(new TargetDataComparer());
        
        public Vector3 FixatedTargetPosition { get; set; } = Vector3.zero;
        
        public void AddTarget(Transform target, float distance)
        {
            targets.Add(new TargetData(distance, target));
        }

        public void Clear()
        {
            targets.Clear();
        }
        
        public Transform GetClosestTarget()
        {
            return targets.Count != 0 
                ? targets.Min
                : null;
        }
        
        public IEnumerator<TargetData> GetTargets()
        {
            return targets.GetEnumerator();
        }
        
        public struct TargetDataComparer : IComparer<TargetData>
        {
            public int Compare(TargetData x, TargetData y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }

        [Serializable]
        public struct TargetData
        {
            public float distance;
            public Transform target;

            public TargetData(float distance, Transform target)
            {
                this.distance = distance;
                this.target = target;
            }
            
            public static implicit operator Transform(TargetData targetData)
            {
                return targetData.target;
            }
        }
    }
}