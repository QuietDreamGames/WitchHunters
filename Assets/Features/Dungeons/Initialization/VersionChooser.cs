using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Dungeons.Initialization
{
    public class VersionChooser : MonoBehaviour
    {
        private void Awake()
        {
            var origin = transform;
            var currentChildIndex = Random.Range(0, origin.childCount);
            for (var i = 0; i < origin.childCount; i++)
            {
                var child = origin.GetChild(i);
                child.gameObject.SetActive(i == currentChildIndex);
            }
        }
    }
}
