using System;
using Features.Camera;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterSpawnPoint : MonoBehaviour
    {
        private CharacterHolder _characterHolder;
        private CameraData _cameraData;

        private void Start()
        {
            _characterHolder = ServiceLocator.Resolve<CharacterHolder>();
            _cameraData = ServiceLocator.Resolve<CameraData>();

            var character = _characterHolder.CurrentCharacter;
            var characterTransform = character.transform;
            characterTransform.position = transform.position;
            
            _cameraData.SetTarget(characterTransform);
        }
    }
}
