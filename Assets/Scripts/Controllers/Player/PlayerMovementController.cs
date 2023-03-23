using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float playerSpeed = 5f;
        [SerializeField]
        private bool canMove = true;

        [Header("Dash")]
        [SerializeField]
        private float dashSpeed = 20f;

        [SerializeField]
        [Tooltip("Dash duration in seconds")]
        private float dashDuration = 0.25f;

        [SerializeField]
        [Tooltip("Dash countdown timer when starting dash")]
        private float dashTimer;

        [SerializeField]
        [Tooltip("Time in seconds for dash to be available again")]
        private float dashCooldown;

        [SerializeField]
        private bool dashAvailable = true;

        [Header("Gamepad")]
        [SerializeField]
        private float controllerDeadzone = 0.1f;

        [SerializeField]
        private float gamepadRotateSmoothing = 1000f;

        [SerializeField]
        private bool isGamepad;

        [Header("Camera")]
        [SerializeField]
        private Camera _camera;

        private CharacterController controller;

        private Vector2 movementInput, lookInput;
        private Vector3 playerVelocity;

        private PlayerInputManager playerInputManager;
        private PlayerInput playerInput;

        private GameInput gameInput;

        //variable for running animation to start
        private bool isRunning;
        private bool dashInput;
        private bool isDashing = false;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            gameInput = FindObjectOfType<GameInput>();
        }

        void Update()
        {
            if (!isDashing)
            {
                HandleInput();
            }
            HandleRotation();
        }

        void FixedUpdate()
        {
            if (canMove)
            {
                HandleMovement();
            }

            HandleDash();
        }

        void HandleInput()
        {
            //get input from player input manager
            movementInput = gameInput.GetMovement();
            lookInput = gameInput.GetLookPosition();
            dashInput = gameInput.GetDash();
        }

        void HandleMovement()
        {
            //set running to true if movement direction is not zero for animation to start
            isRunning = GetMovementDirection() != Vector3.zero;
            //move player
            controller.Move(GetMovementDirection() * Time.deltaTime * playerSpeed);
        }

        public Vector3 GetMovementDirection()
        {
            //create vector3 from input
            Vector3 movementDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            return movementDirection = Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;
        }

        void HandleRotation()
        {
            if (isGamepad)
            {
                float cameraEulerY = _camera.gameObject.transform.eulerAngles.y;
                float rotateTowardsValue = gamepadRotateSmoothing * Time.deltaTime;

                Vector3 lookInputVector = new Vector3(lookInput.x, 0, lookInput.y);
                float lookInputSqrMagnitude = lookInputVector.sqrMagnitude;

                //if input is greater than deadzone, rotate towards input
                if (lookInputSqrMagnitude > controllerDeadzone * controllerDeadzone)
                {
                    //rotate towards input
                    Vector3 playerDirection = Vector3.right * lookInput.x + Vector3.forward * lookInput.y;
                    if (playerDirection.sqrMagnitude > 0.0f)
                    {
                        Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up) * Quaternion.Euler(0, cameraEulerY, 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotateTowardsValue);
                    }
                }
                else
                {
                    KeepRotationAfterMovement();
                }
            }
            else
            {
                KeepRotationAfterMovement();
                // // TODO SHOULD ONLY LOOK AT MOUSE DURING ATTACK
                // //LOOK TOWARDS MOUSE POSITION
                // Ray ray = _camera.ScreenPointToRay(lookInput);
                // Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                // float rayDistance;

                // if (groundPlane.Raycast(ray, out rayDistance))
                // {
                //     Vector3 pointToLook = ray.GetPoint(rayDistance);
                //     LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
                // }
            }

        }

        private void KeepRotationAfterMovement()
        {
            if (GetMovementDirection() != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(GetMovementDirection(), Vector3.up);
            }
        }

        private void LookAt(Vector3 lookPoint)
        {
            //corrects height of look point so player doesn't look up or down
            Vector3 heighCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heighCorrectedPoint);
        }

        public void HandleDash()
        {
            if (!dashAvailable && dashInput)
            {
                //text on screen to tell player dash is on cooldown
            }

            if (dashAvailable && dashInput)
            {
                //when activation input is pressed, start dash cooldown
                StartCoroutine(DashCoroutine());
            }
        }

        private IEnumerator DashCoroutine()
        {
            // disable user input
            isDashing = true;
            dashAvailable = false;
            // set dash timer
            dashTimer = dashDuration;
            Vector3 dashDir = new Vector3(movementInput.x, 0, movementInput.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            dashDir = Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * dashDir;

            while (dashTimer > 0)
            {
                // move player
                controller.Move(dashDir * dashSpeed * Time.deltaTime);
                // decrease dash timer
                dashTimer -= Time.deltaTime;
                yield return null;
            }

            // enable user input after the dash
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            dashAvailable = true;
        }


        public void OnDeviceChange(PlayerInput pi)
        {
            //check if gamepad is connected
            isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}