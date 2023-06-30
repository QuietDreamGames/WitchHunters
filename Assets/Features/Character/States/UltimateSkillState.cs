using Features.FiniteStateMachine.Interfaces;
using Features.Modifiers;

namespace Features.Character.States
{
    public class UltimateSkillState : SkillState
    {
        public UltimateSkillState(IMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();

            CharacterView.PlayUltimateAnimation();

            var cooldownInfo = new ModifierInfo
            {
                Type = ModifierType.UltimateCurrentCooldown,
                TimeType = ModifierTimeType.Temporary,
                Spec = ModifierSpec.RawAdditional,
                Duration = ModifiersController.CalculateModifiedValue(ModifierType.UltimateCooldown),
                MaxDuration = ModifiersController.CalculateModifiedValue(ModifierType.UltimateCooldown),
            };
            
            ModifiersController.AddModifier(cooldownInfo);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CharacterView.IsUltimateAnimationComplete())
            {
                stateMachine.ChangeState("IdleCombatState");
            }
        }
    }
}