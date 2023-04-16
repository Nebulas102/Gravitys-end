using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace Controllers.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class Character : MonoBehaviour
    {
        [Header("Controls")]
        public float playerSpeed = 5.0f;

        public StateMachine movementSM;
        public StandingState standing;
        public SprintState sprinting;
        public CombatState combatting;

        [HideInInspector]
        public CharacterController controller;
        [HideInInspector]
        public PlayerInput playerInput;
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public Vector3 playerVelocity;


        // Start is called before the first frame update
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            movementSM = new StateMachine();
            standing = new StandingState(this, movementSM);
            sprinting = new SprintState(this, movementSM);
            combatting = new CombatState(this, movementSM);

            movementSM.Initialize(standing);
        }

        private void Update()
        {
            movementSM.currentState.HandleInput();

            movementSM.currentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            movementSM.currentState.PhysicsUpdate();
        }
    }
}