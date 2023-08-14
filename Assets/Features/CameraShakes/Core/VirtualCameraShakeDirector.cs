using System.Collections;
using Cinemachine;
using Features.TimeSystems.Interfaces.Handlers;
using Unity.Mathematics;
using UnityEngine;

namespace Features.CameraShakes.Core
{
    public class VirtualCameraShakeDirector : MonoBehaviour, IShakeDirector, IUpdateHandler
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        [Header("Builders")]
        [SerializeField] private NoiseSettingsBuilder noiseSettingsBuilder;
        
        private CinemachineBasicMultiChannelPerlin _perlin;
        
        private Coroutine _shakeRoutine;
        private Coroutine _finishShakeRoutine;
        
        private float _deltaTime;

        private void Awake()
        {
            _perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        public void RegisterShakeData(CameraShakeData data)
        {
            if (_shakeRoutine != null)
            {
                StopCoroutine(_shakeRoutine);
                _shakeRoutine = null;
            }
            
            if (_finishShakeRoutine != null)
            {
                StopCoroutine(_finishShakeRoutine);
                _finishShakeRoutine = null;
            }
            
            _shakeRoutine = StartCoroutine(ShakeRoutine(data));
        }
        
        public void UnregisterShakeData(CameraShakeData data)
        {
            if (_shakeRoutine != null)
            {
                StopCoroutine(_shakeRoutine);
                _shakeRoutine = null;
            }
            
            if (_finishShakeRoutine != null)
            {
                StopCoroutine(_finishShakeRoutine);
                _finishShakeRoutine = null;
            }
            
            _shakeRoutine = StartCoroutine(ShakeRoutine(data));
        }

        private IEnumerator ShakeRoutine(CameraShakeData data)
        {
            _perlin.m_AmplitudeGain = data.amplitude;
            _perlin.m_FrequencyGain = data.frequency;
            _perlin.m_NoiseProfile = noiseSettingsBuilder.Build(data.shakeDirection);
            
            var time = 0f;
            while (time < data.duration)
            {
                time += _deltaTime;
                yield return null;
            }
            
            if (_finishShakeRoutine != null)
            {
                StopCoroutine(_finishShakeRoutine);
                _finishShakeRoutine = null;
            }
            
            _finishShakeRoutine = StartCoroutine(FinishShakeRoutine());
        }
        
        private IEnumerator FinishShakeRoutine()
        {
            const float duration = .1f;
            var time = 0f;
            
            var startAmplitude = _perlin.m_AmplitudeGain;
            var startFrequency = _perlin.m_FrequencyGain;
            
            while (time < duration)
            {
                time += _deltaTime;
                var t = time / duration;
                
                _perlin.m_AmplitudeGain = math.lerp(startAmplitude, 0, t);
                _perlin.m_FrequencyGain = math.lerp(startFrequency, 15, t);

                yield return null;
            }
            
            _perlin.m_AmplitudeGain = 0;
            _perlin.m_FrequencyGain = 15;
        }

        public void OnUpdate(float deltaTime)
        {
            _deltaTime = deltaTime;
        }
    }
}
