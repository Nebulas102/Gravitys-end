using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Player
{
    public class State
    {
        public Character character;
        public StateMachine stateMachine;

        protected Vector3 moveDirection; //WAS VELOCITY
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
            dashAction = character.playerInput.actions["Dash"]; 
        }

        public virtual void Enter()
        {
            // Debug.Log("Enter State: " + this.ToString());
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