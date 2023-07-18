using Features.Modifiers;
using Features.Modifiers.SOLID.Core;
using Features.Modifiers.SOLID.Helpers;
using Features.Skills.Core;
using UnityEngine;

namespace Features.Skills.Implementations
{
    public class InqPassiveController : APassiveController
    {
        private float _delayDecay;
        private float _delayDecayTimer;

        public override void Initiate(ModifiersContainer modifiersContainer, BaseModifiersContainer baseModifiersContainer)
        {
            base.Initiate(modifiersContainer, baseModifiersContainer);
            
            _delayDecay = modifiersContainer.GetValue(ModifierType.PassiveDelayDecay, baseModifiersContainer.GetBaseValue(ModifierType.PassiveDelayDecay));
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (CurrentPassiveInfo.IsCharged)
            {
                CurrentPassiveInfo.CurrentTimer -= deltaTime;
                // var maxTimer = modifiersContainer.GetValue(ModifierType.PassiveChargedTime,
                //     baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargedTime));
                //
                // CurrentPassiveInfo.CurrentCharge = CurrentPassiveInfo.CurrentTimer / maxTimer;
                
                if (CurrentPassiveInfo.CurrentTimer <= 0)
                {
                    CurrentPassiveInfo.IsCharged = false;
                    CurrentPassiveInfo.CurrentTimer = 0;
                    CurrentPassiveInfo.CurrentCharge = 0;
                }
            }
            else
            {
                if (_delayDecayTimer <= 0)
                {
                    CurrentPassiveInfo.CurrentCharge = Mathf.Clamp(CurrentPassiveInfo.CurrentCharge - deltaTime, 0, CurrentPassiveInfo.CurrentCharge - deltaTime);
                    _delayDecayTimer = 0;
                }

                else
                {
                    _delayDecayTimer -= deltaTime;
                }
            }
        }
        
        public override void OnHit()
        {
            base.OnHit();
            
            if (CurrentPassiveInfo.IsCharged) return;

            CurrentPassiveInfo.CurrentCharge += modifiersContainer.GetValue(ModifierType.PassiveChargePerHit, baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargePerHit));
            
            if (CurrentPassiveInfo.CurrentCharge >= modifiersContainer.GetValue(ModifierType.PassiveAmountToCharge, baseModifiersContainer.GetBaseValue(ModifierType.PassiveAmountToCharge)))
            {
                CurrentPassiveInfo.IsCharged = true;
                CurrentPassiveInfo.CurrentTimer = modifiersContainer.GetValue(ModifierType.PassiveChargedTime, baseModifiersContainer.GetBaseValue(ModifierType.PassiveChargedTime));
            }
            else
            {
                _delayDecay = modifiersContainer.GetValue(ModifierType.PassiveDelayDecay, baseModifiersContainer.GetBaseValue(ModifierType.PassiveDelayDecay));
                _delayDecayTimer = _delayDecay;
            }

        }
        
    }
}