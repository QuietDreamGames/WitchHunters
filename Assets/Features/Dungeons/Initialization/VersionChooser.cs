using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Dungeons.Initialization
{
    public class VersionChooser : MonoBehaviour
    {
        private const string IgnoreChild = "BASE";
        
        private void Awake()
        {
            var origin = transform;
            var children = new List<GameObject>();
            
            for (var i = 0; i < origin.childCount; i++)
            {
                var child = origin.GetChild(i);
                if (child.name == IgnoreChild) 
                    continue;
                children.Add(child.gameObject);
            }
            
            var currentChildIndex = Random.Range(0, children.Count);
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                child.SetActive(i == currentChildIndex);
            }
        }
    }
}
