using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Controllers.Player
{
    public class DashingState : State
    {
        private Vector3 currentVelocity;
        private float playerSpeed;
        private bool dashing;
        private bool _dashInput;
        private Vector3 dashDirection;

        public DashingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();

            dashing = false;
            input = Vector2.zero;
            moveDirection = Vector3.zero;
            currentVelocity = Vector3.zero;

            playerSpeed = character.playerSpeed;
        }

        public override void HandleInput()
        {
            base.Enter();
            input = moveAction.ReadValue<Vector2>();
            dashDirection = new Vector3(input.x, 0, input.y);

            if (dashAction.triggered)
            {
                dashing = true;
            }
            else
            {
                dashing = false;
            }

            dashDirection = Quaternion.Euler(0, character._camera.gameObject.transform.eulerAngles.y, 0) * dashDirection;


        }

        public override void LogicUpdate()
        {
            if (dashing)
            {
                // character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
                stateMachine.ChangeState(character.dashing);
            }
            else
            {
                stateMachine.ChangeState(character.standing);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            // currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
            Debug.Log("DASh!!!!!");
            character.controller.Move(dashDirection * (character.dashSpeed * Time.deltaTime));


            // if (velocity.sqrMagnitude > 0)
            // {
            //     character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
            // }
        }
        public override void Exit()
        {
            base.Exit();
            
        }
    }
}