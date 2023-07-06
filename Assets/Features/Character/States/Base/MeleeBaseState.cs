using Features.FiniteStateMachine;
using Features.FiniteStateMachine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Character.States.Base
{
    public class MeleeBaseState : State
    {
        protected CharacterView CharacterView;
        protected PlayerInput PlayerInput;
        protected bool ShouldCombo;
        protected int attackIndex;
        
        // private float _attackPressedTimer = 0;

        protected Collider2D HitCollider;
        // private List<Collider2D> _collidersDamaged;

        public MeleeBaseState(IMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            CharacterView = stateMachine.GetExtension<CharacterView>();
            PlayerInput = stateMachine.GetExtension<PlayerInput>();
            // _collidersDamaged = new List<Collider2D>();
            // HitCollider = stateMachine.GetExtension<Collider2D>();
        }
        
        public override void OnUpdate(float deltaTime)
        {
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
            
            // if (CharacterView.IsAttackColliderActive())
            // {
            //     Attack();
            // }
            
            ShouldCombo = PlayerInput.actions["Attack"].IsPressed();
        }

        public override void OnFixedUpdate(float deltaTime)
        {
            
        }

        public override void OnLateUpdate(float deltaTime)
        {
            
        }
        
        public override void OnExit()
        {
            
        }
        
        // protected void Attack()
        // {
        //     Collider2D[] colliders = new Collider2D[10];
        //     ContactFilter2D contactFilter2D = new ContactFilter2D();
        //     contactFilter2D.useTriggers = true;
        //     int colliderCount = HitCollider.OverlapCollider(contactFilter2D, colliders);
        //     
        //     for (int i = 0; i < colliderCount; i++)
        //     {
        //         Collider2D collider = colliders[i];
        //         if (!_collidersDamaged.Contains(collider))
        //         {
        //             TeamComponent hitTeamComponent = collider.GetComponent<TeamComponent>();
        //             
        //             if (hitTeamComponent != null && hitTeamComponent.TeamIndex == TeamIndex.Enemy)
        //             {
        //                 _collidersDamaged.Add(collider);
        //                 Debug.Log($"{collider.name} was damaged");
        //             }
        //         }
        //     }
        //
        // }
    }
}