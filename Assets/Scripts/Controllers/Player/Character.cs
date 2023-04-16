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
        [Header("Movement")]
        public float playerSpeed = 10f;

        [Header("Dash")]
        public float dashSpeed = 20f;
        public float dashTime = 0.5f;
        public float dashCooldown = 3f;
        public bool dashAvailable = true;

        [Header("Gamepad")]
        public float controllerDeadzone = 0.1f;
        public float gamepadRotateSmoothing = 1000f;
        public bool isGamepad;

        [Header("Camera")]
        [SerializeField]
        public Camera _camera;

        public Vector3 playerVelocity;


        public StateMachine movementSM;

        public StandingState standing;
        public MovingState moving;
        public DashingState dashing;

        [HideInInspector]
        public CharacterController controller;
        [HideInInspector]
        public PlayerInput playerInput;
        [HideInInspector]
        public Animator animator;




        // Start is called before the first frame update
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            movementSM = new StateMachine();
            standing = new StandingState(this, movementSM);
            moving = new MovingState(this, movementSM);
            dashing = new DashingState(this, movementSM);

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

        public void OnDeviceChange(PlayerInput pi)
        {
            //check if gamepad is connected
            isGamepad = pi.currentControlScheme.Equals("Gamepad");
            
        }
    }
}