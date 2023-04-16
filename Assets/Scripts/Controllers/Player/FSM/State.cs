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
        public InputAction attackAction;


        public State(Character _character, StateMachine _stateMachine)
        {
            character = _character;
            stateMachine = _stateMachine;

            moveAction = character.playerInput.actions["Move"];
            lookAction = character.playerInput.actions["Look"];
            attackAction = character.playerInput.actions["Attack"];
        }

        public virtual void Enter()
        {
            //StateUI.instance.SetStateText(this.ToString());
            Debug.Log("Enter State: " + this.ToString());
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