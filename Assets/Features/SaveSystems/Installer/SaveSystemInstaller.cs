using Features.SaveSystems.Core;
using Features.SaveSystems.Interfaces;
using Features.SaveSystems.Modules.PathBuilder;
using Features.SaveSystems.Modules.Serializer;
using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.SaveSystems.Installer
{
    public class SaveSystemInstaller : ServiceInstaller
    {
        [SerializeField] private SaveSystem saveSystemSystem;
        
        public override void Install()
        {
            var savablePathBuilder = new SavablePathBuilder();
            var savableSerializer = new SavableJsonSerializer();
            
            ServiceLocator.Register<ISavablePathBuilder>(savablePathBuilder);
            ServiceLocator.Register<ISavableSerializer>(savableSerializer);
            ServiceLocator.Register<SaveSystem>(saveSystemSystem);
            
            saveSystemSystem.Construct();
        }
    }
}