using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Presentation.Feature
{
    public class Fps : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsCounter;
        [SerializeField] private TextMeshProUGUI _avgFpsCounter;

        private float[] _frames;
        private int _frameIndex = 0;

        private const int FrameCount = 60;

        private void Start()
        {
            _frames = new float[FrameCount];
            _frameIndex = 0;
            
            var deltaTime = Time.unscaledDeltaTime;

            for (var i = 0; i < FrameCount; i++)
            {
                _frames[i] = deltaTime;
            }
            
            Recalculate();
        }

        private void Update()
        {
            Recalculate();
        }

        private void  Recalculate()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var fps = 1.0f / deltaTime;

            _frameIndex = (_frameIndex + 1) % FrameCount;
            _frames[_frameIndex] = fps;

            var sum = _frames.Sum();
            var avgFps = sum / FrameCount;

            _fpsCounter.text = $"Ms: {deltaTime * 1000}, FPS: {fps}";
            _avgFpsCounter.text = $"Average FPS: {avgFps}";
        }
        
    }
}
