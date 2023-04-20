using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Player
{
    public class CombatState : State
    {
        bool sheathWeapon;
        float playerSpeed;
        bool attack;

        public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();

            sheathWeapon = false;
            attack = false;
            input = Vector2.zero;

            velocity = character.playerVelocity;
            playerSpeed = character.playerSpeed;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (drawWeaponAction.triggered)
            {
                sheathWeapon = true;
            }

            if (attackAction.triggered)
            {
                attack = true;
            }

            input = moveAction.ReadValue<Vector2>();
            velocity = new Vector3(input.x, 0, input.y);

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            PlayerAnimator.Instance._animator.SetFloat("Velocity", input.magnitude, 0.2f, Time.deltaTime);

            if (sheathWeapon)
            {
                PlayerAnimator.Instance._animator.SetTrigger("sheathWeapon");
                EquipmentSystem.Instance.Invoke("SheathWeapon", 1f);
                
                stateMachine.ChangeState(character.standing);
            }

            if (attack)
            {
                PlayerAnimator.Instance._animator.SetTrigger("attack");
                stateMachine.ChangeState(character.attacking);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            velocity = Quaternion.Euler(0, -45, 0) * velocity;

            character.controller.Move(velocity * Time.deltaTime * playerSpeed);

            if (velocity.sqrMagnitude > 0)
            {
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                    Quaternion.LookRotation(velocity), 0.2f);
            }
        }

        public override void Exit()
        {
            base.Exit();

            character.playerVelocity = new Vector3(input.x, 0, input.y);

            if (velocity.sqrMagnitude > 0) character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}


