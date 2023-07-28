using Features.ServiceLocators.Core;
using Features.ServiceLocators.Core.Service;

namespace Features.Character.Spawn
{
    public class CharacterHolderInstaller : ServiceInstaller
    {
        public override void Install()
        {
            ServiceLocator.Register(new CharacterHolder());
        }
    }
}