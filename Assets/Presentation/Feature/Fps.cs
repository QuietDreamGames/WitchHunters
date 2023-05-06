using System.Collections;
using UnityEngine;

namespace Presentation.Feature
{
    public class Fps : MonoBehaviour
    {
        private float count;
    
        private IEnumerator Start()
        {
            GUI.depth = 2;
            while (true)
            {
                count = 1f / Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
        }
    
        private void OnGUI()
        {
            GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(count));
        }
    }
}
