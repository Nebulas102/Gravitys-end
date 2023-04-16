using UnityEngine;

namespace Controllers.Player
{
    public class StandingState : State
    {
        private float playerSpeed;
        private bool sprint;

        public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            sprint = false;
            input = Vector2.zero;

            velocity = character.playerVelocity;
            playerSpeed = character.playerSpeed;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            if (moveAction.triggered) sprint = true;

            input = moveAction.ReadValue<Vector2>();
            velocity = new Vector3(input.x, 0, input.y);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // PlayerAnimator.Instance.PlayIdle();
            PlayerAnimator.Instance._animator.SetFloat("Velocity", input.magnitude, 0.2f, Time.deltaTime);

            if (sprint) stateMachine.ChangeState(character.sprinting);
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
