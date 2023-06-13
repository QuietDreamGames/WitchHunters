using System.Collections.Generic;
using Features.FiniteStateMachine;
using Features.Team;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States
{
    public class MeleeBaseState : State
    {
        protected CharacterView CharacterView;
        protected PlayerInput PlayerInput;
        protected bool ShouldCombo;
        protected int attackIndex;
        
        private float _attackPressedTimer = 0;

        protected Collider2D HitCollider;
        private List<Collider2D> _collidersDamaged;

        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            CharacterView = stateMachine.GetExtension<CharacterView>();
            PlayerInput = stateMachine.GetExtension<PlayerInput>();
            _collidersDamaged = new List<Collider2D>();
            HitCollider = StateMachine.GetExtension<Collider2D>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // _attackPressedTimer -= Time.deltaTime;
            //
            // if (CharacterView.IsAttackColliderActive())
            // {
            //     Attack();
            // }
            //
            // if (PlayerInput.actions["Attack"].IsPressed())
            // {
            //     _attackPressedTimer = 1f;
            // }
            //
            // ShouldCombo = _attackPressedTimer > 0;
            
            if (CharacterView.IsAttackColliderActive())
            {
                Attack();
            }
            
            ShouldCombo = PlayerInput.actions["Attack"].IsPressed();
        }
        
        protected void Attack()
        {
            Collider2D[] colliders = new Collider2D[10];
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            int colliderCount = HitCollider.OverlapCollider(contactFilter2D, colliders);
            
            for (int i = 0; i < colliderCount; i++)
            {
                Collider2D collider = colliders[i];
                if (!_collidersDamaged.Contains(collider))
                {
                    TeamComponent hitTeamComponent = collider.GetComponent<TeamComponent>();
                    
                    if (hitTeamComponent != null && hitTeamComponent.TeamIndex == TeamIndex.Enemy)
                    {
                        _collidersDamaged.Add(collider);
                        Debug.Log($"{collider.name} was damaged");
                    }
                }
            }

        }
    }
}