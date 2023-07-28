using System;
using Features.Character.Spawn;
using Features.ServiceLocators.Core;
using UnityEngine;

namespace Features.Floor
{
    public class FloorController : MonoBehaviour
    {
        [SerializeField] private CharacterSpawner _characterSpawner;
        
        private void Start()
        {
            var character = _characterSpawner.SpawnCharacter();
            ServiceLocator.Resolve<CharacterHolder>().SetCurrentCharacter(character);
        }

        public void RestartFloor()
        {
            Debug.LogError("TRIED TO RESTART THE FLOOR, NOT YET IMPLEMENTED");
        }
    }
}