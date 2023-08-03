using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;

namespace Features.Character.Spawn
{
    public class CharacterHolderInstaller : ServiceInstaller
    {
        [SerializeField] private CharacterHolder characterHolder;
        
        public override void Install()
        {
            ServiceLocator.Register<CharacterHolder>(characterHolder);
        }
    }
}