using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Player
{
    public class AttackState : State
    {
        private float timePassed;
        private bool attackTriggered;
        private Animator animator;
        private const float ComboDelay = 0.6f;

        public AttackState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
            animator = PlayerAnimator.Instance._animator; 
        }

        public override void Enter()
        {
            base.Enter();
            attackTriggered = false;
            timePassed = 0f;
            animator.SetTrigger("attack");
        }

        public override void HandleInput()
        {
            base.HandleInput();

            // Handle input for attack action
            if (attackAction.triggered)
            {
                attackTriggered = true;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            timePassed += Time.deltaTime;

            // Calculate combo length based on the clip length and speed
            float clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
            float comboLength = clipLength / clipSpeed;

            // Check if attack triggered during combo delay
            if (timePassed < ComboDelay && attackTriggered)
            {
                timePassed = 0f;
                stateMachine.ChangeState(character.attacking);
            }

            // Check if combo length has passed
            if (timePassed >= comboLength)
            {
                animator.SetTrigger("move");
                stateMachine.ChangeState(character.combatting);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}