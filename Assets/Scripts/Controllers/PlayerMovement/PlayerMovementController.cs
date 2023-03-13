using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers.PlayerMovement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float playerSpeed = 5f;

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

        //variable for running animation to start
        private bool isRunning;
        private bool dashInput;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInputManager = new PlayerInputManager();
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            playerInputManager.Enable();
        }

        private void OnDisable()
        {
            playerInputManager.Disable();
        }

        void Update()
        {
            HandleInput();
            HandleRotation();
        }

        void FixedUpdate()
        {
            HandleMovement();
            HandleDash();
        }

        void HandleInput()
        {
            movementInput = playerInputManager.Player.Move.ReadValue<Vector2>();
            lookInput = playerInputManager.Player.Look.ReadValue<Vector2>();
            dashInput = playerInputManager.Player.Dash.ReadValue<float>() > 0;
        }

        void HandleMovement()
        {
            //set running to true if movement direction is not zero for animation to start
            isRunning = GetMovementDirection() != Vector3.zero;

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


                if (lookInputSqrMagnitude > controllerDeadzone * controllerDeadzone)
                {
                    Vector3 playerDirection = Vector3.right * lookInput.x + Vector3.forward * lookInput.y;
                    if (playerDirection.sqrMagnitude > 0.0f)
                    {
                        Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up) * Quaternion.Euler(0, cameraEulerY, 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotateTowardsValue);
                    }
                }
                else
                {
                    if (GetMovementDirection() != Vector3.zero)
                    {
                        transform.rotation = Quaternion.LookRotation(GetMovementDirection(), Vector3.up);
                    }
                }
            }
            else
            {
                //mouse rotation
                Ray ray = _camera.ScreenPointToRay(lookInput);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 pointToLook = ray.GetPoint(rayDistance);
                    LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
                }
            }

        }

        private void LookAt(Vector3 lookPoint)
        {
            //corrects height of look point so player doesn't look up or down
            Vector3 heighCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heighCorrectedPoint);
        }

        //get dash input and dash player in direction of input
        public void HandleDash()
        {
            if (!dashAvailable && dashInput)
            {
                //text on screen to tell player dash is on cooldown
            }

            if (dashAvailable && dashInput)
            {
                //when activation input is pressed, start dash cooldown
                StartCoroutine(DashCooldown());
            }
            if (dashTimer > 0)
            {
                //when dash timer is greater than 0, dash player
                Dash();
            }
        }

        //dash
        IEnumerator DashCooldown()
        {
            dashAvailable = false;
            dashTimer = dashDuration;
            yield return new WaitForSeconds(dashCooldown);
            dashAvailable = true;
        }

        private void Dash()
        {
            //get direction of dash
            Vector3 dashDir = new Vector3(movementInput.x, 0, movementInput.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            dashDir = Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * dashDir;
            //start duration timer
            dashTimer -= Time.deltaTime;
            //move player in direction of dash
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
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





        // [SerializeField]
        // private GameInput gameInput;



        // private Rigidbody rb;
        // private bool dashInput;
        // private Vector2 move, moveInput, lookDir, joystickLook;

        // void Awake()
        // {
        //     //freeze rotation so player doesn't fall over
        //     rb = GetComponent<Rigidbody>();
        //     rb.freezeRotation = true;
        // }

        // void Update()
        // {
        //     moveInput = gameInput.GetMovementVectorNormalized();
        //     lookDir = gameInput.GetLookPosition();
        //     dashInput = gameInput.GetDash();
        // }

        // void FixedUpdate()
        // {
        //     OnMove();
        //     OnLook();
        //     OnDash();
        // }



        // public Vector3 GetMovementDirection()
        // {
        //     //gets input from player
        //     Vector2 inputVector = moveInput;
        //     //create vector3 from input
        //     Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        //     //makes player move independent of camera rotation (W means north, S means south, etc.)
        //     return movementDirection = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;
        // }


        // public void OnMove()
        // {
        //     //get movement direction
        //     Vector3 movementDirection = GetMovementDirection();
        //     //set running to true if movement direction is not zero for animation to start
        //     isRunning = movementDirection != Vector3.zero;
        //     //move character to new position with set speed
        //     rb.MovePosition(transform.position + movementDirection * speed * Time.deltaTime);
        // }

        // //rotate player to face mouse/joystick position
        // private void OnLook()
        // {
        //     if (lookDir == Vector2.zero)
        //     {
        //         return;
        //     }
        //     //mouse
        //     if (lookDir.x > 1 || lookDir.y > 1)
        //     {
        //         Ray cameraRay = camera.ScreenPointToRay(lookDir);
        //         Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        //         float rayLength;

        //         if (groundPlane.Raycast(cameraRay, out rayLength))
        //         {
        //             Vector3 pointToLook = cameraRay.GetPoint(rayLength);
        //             transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        //         }
        //     }
        //     //controller
        //     else if (lookDir.x <= 1 || lookDir.y <= 1 || lookDir.x <= -1 || lookDir.y <= -1)
        //     {
        //         Vector3 aimDirection = new Vector3(lookDir.x, 0, lookDir.y);

        //         if (aimDirection != Vector3.zero)
        //         {
        //             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 1f) * Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0);
        //         }
        //     }
        // }


    }
}