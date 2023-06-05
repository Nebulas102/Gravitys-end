using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Player
{
    public class CombatState : State
    {
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


            attack = false;
            input = Vector2.zero;

            velocity = character.playerVelocity;
            playerSpeed = character.playerSpeed;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (attackAction.triggered)
            {
                attack = true;

                if (EquipmentSystem.Instance.currentWeaponInHand.GetComponent<MeleeWeapon>())
                {
                    EquipmentSystem.Instance.currentWeaponInHand.GetComponent<MeleeWeapon>().AllowHitbox();
                }
            }

            input = moveAction.ReadValue<Vector2>();
            velocity = new Vector3(input.x, 0, input.y);

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            PlayerAnimator.Instance._animator.SetFloat("Velocity", input.magnitude, 0.2f, Time.deltaTime);

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


