using UnityEngine;

namespace Controllers.Player
{
    public class MovingState : State
    {
        private Vector3 currentVelocity;
        private Vector3 cVelocity;
        private bool moving;
        private bool dashing;
        private float playerSpeed;

        public MovingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
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
            moveDirection = Vector3.zero;

            currentVelocity = character.controller.velocity;
            playerSpeed = character.playerSpeed;

        }

        public override void HandleInput()
        {
            base.Enter();

            input = moveAction.ReadValue<Vector2>();
            moveDirection = new Vector3(input.x, 0, input.y);

            if (dashAction.triggered)
            {
                dashing = true;
            }
        }

        public override void LogicUpdate()
        {
            if (dashing)
            {
                stateMachine.ChangeState(character.dashing);
            }

            if (moving)
            {
                // play animation
                // character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
            }
            else
            {
                stateMachine.ChangeState(character.standing);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            moveDirection = Quaternion.Euler(0, -45, 0) * moveDirection;
            character.controller.Move(moveDirection * (Time.deltaTime * playerSpeed));


            // if (velocity.sqrMagnitude > 0)
            // {
            //     character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
            // }
        }
    }
}