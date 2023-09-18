using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Dungeons.Initialization
{
    public class VersionChooser : MonoBehaviour
    {
        private const string IgnoreChild = "BASE";
        
        private void Start()
        {
            var origin = transform;
            var children = new List<GameObject>();
            
            for (var i = 0; i < origin.childCount; i++)
            {
                var child = origin.GetChild(i).gameObject;
                child.SetActive(false);
                if (child.name == IgnoreChild) 
                    continue;
                children.Add(child);
            }
            
            var currentChildIndex = Random.Range(0, children.Count);
            children[currentChildIndex].SetActive(true);
        }
    }
}
