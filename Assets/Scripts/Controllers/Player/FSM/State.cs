using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Player
{
    public class State
    {
        public Character character;
        public StateMachine stateMachine;

        protected Vector3 velocity;
        protected Vector2 input;

        public InputAction moveAction;
        public InputAction lookAction;
        public InputAction dashAction;


        public State(Character _character, StateMachine _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;

            moveAction = character.playerInput.actions["Move"];
            lookAction = character.playerInput.actions["Look"];
        }

        public virtual void Enter()
        {

        }

        public virtual void HandleInput()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Exit()
        {
        }
    }
}