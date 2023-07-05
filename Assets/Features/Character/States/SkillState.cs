using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;

namespace Features.Character.States
{
    public class SkillState : State
    {
        protected CharacterView CharacterView;
        protected ModifiersContainer ModifiersContainer;
        protected BaseModifiersContainer BaseModifiersContainer;
        
        public SkillState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            CharacterView = stateMachine.GetExtension<CharacterView>();
            ModifiersContainer = stateMachine.GetExtension<ModifiersContainer>();
            BaseModifiersContainer = stateMachine.GetExtension<BaseModifiersContainer>();
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnFixedUpdate(float deltaTime)
        {
        }

        public override void OnLateUpdate(float deltaTime)
        {
        }
    }
}