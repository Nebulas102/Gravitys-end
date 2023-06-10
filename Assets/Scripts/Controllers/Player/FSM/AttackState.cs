using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Player
{
    public class AttackState : State
    {
        float timePassed;
        float clipLength;
        float clipSpeed;
        bool attack;

        private Animator animator = PlayerAnimator.Instance._animator;
        private float _comboDelay = 0.5f;
        private int _numClicks = 0;
        private float _lastClickTime = 0;


        public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            attack = false;
            timePassed = 0;
            animator.SetTrigger("attack");
            animator.SetFloat("Velocity", 0f);
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (Time.time - _lastClickTime < _comboDelay)
            {
                _numClicks = 0;
            }
            if (attackAction.triggered)
            {
                _lastClickTime = Time.time;
                _numClicks++;

                if(_numClicks == 1){
                    animator.SetTrigger("attack");
                }
                _numClicks = Mathf.Clamp(_numClicks, 0, 3);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // timePassed += Time.deltaTime;
            // clipLength = animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
            // clipSpeed = animator.GetCurrentAnimatorStateInfo(1).speed;
            
            // if (timePassed >= clipLength / clipSpeed && attack)
            // {
            //     stateMachine.ChangeState(character.attacking);
            // }

            // if (timePassed >= clipLength / clipSpeed)
            // {
            //     stateMachine.ChangeState(character.combatting);
            //     animator.SetTrigger("move");
            // }
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}