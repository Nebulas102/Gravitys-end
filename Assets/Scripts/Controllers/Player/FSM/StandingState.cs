using UnityEngine;

namespace Controllers.Player
{
    public class StandingState : State
    {
        private Vector3 currentVelocity;
        private Vector3 cVelocity;
        private bool moving;
        private bool dashing;
        private float playerSpeed;


        public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();

            moving = false;
            dashing = false;
            input = Vector2.zero;

            moveDirection = character.playerVelocity;
            playerSpeed = character.playerSpeed;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            input = moveAction.ReadValue<Vector2>();
            if (input.sqrMagnitude > 0)
            {
                moving = true;
            }

            moveDirection = new Vector3(input.x, 0, input.y);

        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // play animation
            // character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

            if (moving)
            {
                stateMachine.ChangeState(character.moving);
            }


        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            moveDirection = Quaternion.Euler(0, -45, 0) * moveDirection;
            character.controller.Move(moveDirection * (Time.deltaTime * playerSpeed));

            // if (moveDirection.sqrMagnitude > 0)
            // {
            //     character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(moveDirection), character.gamepadRotateSmoothing);
            // }

        }

        public override void Exit()
        {
            base.Exit();

            character.playerVelocity = new Vector3(input.x, 0, input.y);

            if (moveDirection.sqrMagnitude > 0)
            {
                character.transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }
}
