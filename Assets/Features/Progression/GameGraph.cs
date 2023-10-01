using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Progression
{
    public class GameGraph : MonoBehaviour
    {
        [SerializeField] private Connection[] connections;

        public List<int> GetReached(int completed)
        {
            var reached = new List<int>();
            
            for (var i = 0; i < connections.Length; ++i)
            {
                if (connections[i].onComplete == completed)
                {
                    reached.Add(connections[i].setReached);
                }
            }
            
            return reached;
        }
        
        [Serializable]
        public class Connection
        {
            public int onComplete;
            public int setReached;
        }
    }
}
