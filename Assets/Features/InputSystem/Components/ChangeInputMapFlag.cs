using Unity.Entities;

namespace Features.InputSystem.Components
{
    public class ChangeInputMapFlag : IComponentData
    {
        public int NewInputMapId;
    }
}