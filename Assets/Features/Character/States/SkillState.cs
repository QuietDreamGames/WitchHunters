using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;

namespace Features.Character.States
{
    public class SkillState : State
    {
        protected CharacterView CharacterView;
        protected ModifiersController ModifiersController;
        
        public SkillState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            CharacterView = stateMachine.GetExtension<CharacterView>();
            ModifiersController = stateMachine.GetExtension<ModifiersController>();
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