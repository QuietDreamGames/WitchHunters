using System;
using UnityEngine;

namespace Packages.OSSC.Scripts.Core
{
    [Serializable]
    public class SoundData
    {
        public bool active = true;
        public string category = string.Empty;
        public string soundName = string.Empty;

        public ISoundCue Cue => _cue;
        
        private ISoundCue _cue;
        
        public void PlaySoundData(SoundController soundController, Vector3 position, float fadeIn = 0f, float fadeOut = 0f)
        {
            if (active == false)
            {
                return;
            }

            if (soundController == null)
            {
                Debug.LogWarning("soundController is null");
                return;
            }

            _cue = BuildCue(soundController, category, soundName, position, _cue, fadeIn, fadeOut);
        }

        public void PlaySoundData(SoundController soundController, MonoBehaviour mono, float fadeIn = 0f, float fadeOut = 0f)
        {
            if (active == false)
            {
                return;
            }

            if (soundController == null)
            {
                Debug.LogWarning("soundController is null");
                return;
            }

            _cue = BuildCue(soundController, category, soundName, mono.transform, _cue, fadeIn, fadeOut);
        }

        public bool IsPlaying()
        {
            if (_cue == null)
            {
                return false;
            }

            return _cue.IsPlaying;
        }

        public void SetLoop(bool loop)
        {
            if (active == false)
            {
                return;
            }
            
            if (_cue != null && _cue.AudioObject != null)
            {
                _cue.AudioObject.source.loop = loop;
            }
        }

        public void Stop()
        {
            if (_cue != null)
            {
                if (_cue.AudioObject != null && _cue.AudioObject.source != null)
                {
                    _cue.AudioObject.source.loop = false;
                }
                
                _cue.Stop();
            }
        }

        private static ISoundCue BuildCue(
            SoundController soundController,
            string categoryName,
            string soundName,
            Vector3 position,
            ISoundCue soundQueue = null,
            float fadeIn = 0f,
            float fadeOut = 0f)
        {
            var settings = new PlaySoundSettings();
            settings.Init();
            settings.categoryName = categoryName;
            settings.name = soundName;
            settings.soundCueProxy = soundQueue;
            settings.worldPosition = position;
            settings.fadeInTime = fadeIn;
            settings.fadeOutTime = fadeOut;
            if (soundQueue != null) soundQueue.Stop();
            var sQueue = soundController.Play(settings);
            return sQueue;
        }
        
        private static ISoundCue BuildCue(
            SoundController soundController,
            string categoryName,
            string soundName,
            Transform transform,
            ISoundCue soundQueue = null,
            float fadeIn = 0f,
            float fadeOut = 0f)
        {
            var settings = new PlaySoundSettings();
            settings.Init();
            settings.categoryName = categoryName;
            settings.name = soundName;
            settings.soundCueProxy = soundQueue;
            settings.parent = transform;
            settings.fadeInTime = fadeIn;
            settings.fadeOutTime = fadeOut;
            if (soundQueue != null) soundQueue.Stop();
            var sQueue = soundController.Play(settings);
            return sQueue;
        }
    }
}
