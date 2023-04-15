using System.Collections;
using Core.UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerMovementController : MonoBehaviour
    {
        //Animation states
        private const string IDLE = "Idle";
        private const string RUNNING = "Running";
        private const string ATTACK1 = "Attack1";
        private const string ATTACK2 = "Attack2";
        private const string ATTACK3 = "Attack3";
        private static int _noOfClicks = 0;

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

        [SerializeField]
        private bool isDashing;

        [SerializeField]
        private bool isAttacking;

        private Animator _anim;

        private CharacterController _controller;
        private bool _dashInput, _attackInput;

        private GameInput _gameInput;
        private float _lastClickedTime = 0;
        private float _maxComboDelay = 1f;

        private Vector2 _movementInput, _lookInput;
        private Inventory inventory;

        //Attack variables
        private float _nextFireTime = 0f;
        private PlayerInput _playerInput;

        private PlayerInputManager _playerInputManager;
        private Vector3 _playerVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _gameInput = FindObjectOfType<GameInput>();
        }

        private void Start()
        {
            _anim = PlayerAnimator.instance.GetComponent<Animator>();
        }

        private void Update()
        {
            if (!isDashing) HandleInput();
            if (!isAttacking) HandleRotation();

            // If the inventory is opened, prevent that the player can move, otherwise the player can move freely
            canMove = Inventory.Instance.inventoryOpened ? false : true;

            HandleAttack();
        }

        private void FixedUpdate()
        {
            if (canMove && !isAttacking) HandleMovement();
            HandleDash();
        }

        public void OnDeviceChange(PlayerInput pi)
        {
            //check if gamepad is connected
            isGamepad = pi.currentControlScheme.Equals("Gamepad");
        }

        private void HandleInput()
        {
            //get input from player input manager
            _movementInput = _gameInput.GetMovement();
            _lookInput = _gameInput.GetLookPosition();
            _dashInput = _gameInput.GetDash();
            _attackInput = _gameInput.GetAttack();
        }


        //////////////////////////////////////////////
        // Movement                                 // 
        //////////////////////////////////////////////
        private void HandleMovement()
        {
            //play running animation
            PlayerAnimator.instance.ChangeAnimationState(GetMovementDirection() != Vector3.zero ? RUNNING : IDLE);

            //move player
            _controller.Move(GetMovementDirection() * (Time.deltaTime * playerSpeed));
        }

        private Vector3 GetMovementDirection()
        {
            //create vector3 from input
            var movementDirection = new Vector3(_movementInput.x, 0f, _movementInput.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            return movementDirection =
                Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;
        }

        private void HandleRotation()
        {
            if (isGamepad)
            {
                var cameraEulerY = _camera.gameObject.transform.eulerAngles.y;
                var rotateTowardsValue = gamepadRotateSmoothing * Time.deltaTime;

                var lookInputVector = new Vector3(_lookInput.x, 0, _lookInput.y);
                var lookInputSqrMagnitude = lookInputVector.sqrMagnitude;

                //if input is greater than deadzone, rotate towards input
                if (lookInputSqrMagnitude > controllerDeadzone * controllerDeadzone)
                {
                    //rotate towards input
                    var playerDirection = Vector3.right * _lookInput.x + Vector3.forward * _lookInput.y;
                    if (!(playerDirection.sqrMagnitude > 0.0f)) return;

                    var newRotation = Quaternion.LookRotation(playerDirection, Vector3.up) *
                                      Quaternion.Euler(0, cameraEulerY, 0);
                    transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, newRotation, rotateTowardsValue);
                }
                else
                    KeepRotationAfterMovement();
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
                transform.rotation = Quaternion.LookRotation(GetMovementDirection(), Vector3.up);
        }

        private void LookAt(Vector3 lookPoint)
        {
            //corrects height of look point so player doesn't look up or down
            var heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
        }

        //////////////////////////////////////////////
        // Dashing                                  // 
        //////////////////////////////////////////////

        private void HandleDash()
        {
            switch (dashAvailable)
            {
                case false when _dashInput:
                    //text on screen to tell player dash is on cooldown
                    break;
                case true when _dashInput:
                    //when activation input is pressed, start dash cooldown
                    StartCoroutine(DashCoroutine());
                    break;
            }
        }

        private IEnumerator DashCoroutine()
        {
            // disable user input
            isDashing = true;
            dashAvailable = false;
            // set dash timer
            dashTimer = dashDuration;
            var dashDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            dashDir = Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * dashDir;

            while (dashTimer > 0)
            {
                // move player
                _controller.Move(dashDir * (dashSpeed * Time.deltaTime));
                // decrease dash timer
                dashTimer -= Time.deltaTime;
                yield return null;
            }

            // enable user input after the dash
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            dashAvailable = true;
        }

        //////////////////////////////////////////////
        // Attacking                                // 
        //////////////////////////////////////////////
        private void HandleAttack()
        {
            if (!_attackInput) return;

            _attackInput = false;

            if (!isAttacking)
            {
                isAttacking = true;

                PlayerAnimator.instance.ChangeAnimationState(ATTACK1);
            }

            Invoke(nameof(AttackComplete), 1f);
        }

        private void AttackComplete()
        {
            isAttacking = false;
        }

        public float GetDashCooldown()
        {
            return dashCooldown;
        }

        public bool GetDashAvailable()
        {
            return dashAvailable;
        }
    }
}
