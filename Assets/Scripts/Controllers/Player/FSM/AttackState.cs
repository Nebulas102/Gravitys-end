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
            PlayerAnimator.Instance._animator.SetTrigger("attack");
            PlayerAnimator.Instance._animator.SetFloat("Velocity", 0f);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            timePassed += Time.deltaTime;
            clipLength = PlayerAnimator.Instance._animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
            clipSpeed = PlayerAnimator.Instance._animator.GetCurrentAnimatorStateInfo(1).speed;
            
            if (timePassed >= clipLength / clipSpeed && attack)
            {
                stateMachine.ChangeState(character.attacking);
            }

            if (timePassed >= clipLength / clipSpeed)
            {
                stateMachine.ChangeState(character.combatting);
                PlayerAnimator.Instance._animator.SetTrigger("move");
            }
        }

        public override void Exit()
        {
            if (EquipmentSystem.Instance.currentWeaponInHand.GetComponent<MeleeWeapon>())
            {
                EquipmentSystem.Instance.currentWeaponInHand.GetComponent<MeleeWeapon>().DisAllowHitbox();
            }

            base.Exit();
        }

    }
}