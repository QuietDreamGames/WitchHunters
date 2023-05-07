using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Presentation.Feature
{
    public class Fps : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsCounter;
        [SerializeField] private TextMeshProUGUI _avgFpsCounter;
        
        private float deltaTime = 0.0f;
        private float totalFrameTime = 0.0f;
        private float avgFps;
        private int count = 1;

        private void Start()
        {
            avgFps = 1.0f / deltaTime;
            Recalculate();
        }

        private void Update()
        {
            Recalculate();
        }

        private void  Recalculate()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            count++;
            totalFrameTime += fps;
            avgFps = totalFrameTime / count;

            _fpsCounter.text = $"Ms: {msec}, FPS: {fps}";
            _avgFpsCounter.text = $"Average FPS: {avgFps}";
        }
        
    }
}
