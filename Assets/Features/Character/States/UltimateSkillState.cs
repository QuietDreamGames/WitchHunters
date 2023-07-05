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
            
            var cooldown = ModifiersContainer.GetValue(ModifierType.UltimateCooldown,
                BaseModifiersContainer.GetBaseValue(ModifierType.UltimateCooldown));

            ModifiersContainer.Add(ModifierType.UltimateCurrentCooldown, ModifierSpec.RawAdditional, cooldown,
                cooldown);
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