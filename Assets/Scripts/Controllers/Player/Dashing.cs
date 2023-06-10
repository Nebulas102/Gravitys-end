using System.Collections;
using UI;
using UnityEngine;

namespace Controllers.Player
{
    public class Dashing : MonoBehaviour
    {
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

        [SerializeField]
        private bool isDashing;

        private CharacterController _controller;
        private bool _dashInput;
        private GameInput _gameInput;
        private Vector2 _movementInput;
        private bool _gamePaused;

        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _gameInput = FindObjectOfType<GameInput>();
            UIHandler.OnPauseGameToggle += OnPauseGameToggle;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isDashing) HandleInput();
        }

        private void FixedUpdate()
        {
            if (_gamePaused || isDashing || DialogueManager.Instance.dialogueActive)
                return;

            HandleDash();
        }

        private void OnPauseGameToggle(bool gamePaused)
        {
            _gamePaused = gamePaused;
        }

        private void HandleInput()
        {
            Character character = PlayerManager.Instance.player.GetComponent<Character>();
            if (character.movementSM.currentState == character.standing || character.movementSM.currentState == character.attacking)
            {
                return;
            }

            _movementInput = _gameInput.GetMovement();
            if (_movementInput == Vector2.zero)
            {
                return;
            }

            _dashInput = _gameInput.GetDash();
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
            dashDir = Quaternion.Euler(0, -45, 0) * dashDir;
            SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.Dash);

            while (dashTimer > 0)
            {
                // Move player
                _controller.Move(dashDir * (dashSpeed * Time.deltaTime));

                // Decrease dash timer
                dashTimer -= Time.deltaTime;

                yield return null;
            }

            // Enable user input after the dash
            isDashing = false;

            // Wait for dash cooldown
            yield return new WaitForSeconds(dashCooldown);

            // Re-enable dash availability
            dashAvailable = true;
        }

        public float GetDashTimer()
        {
            return dashTimer;
        }

        public bool GetDashAvailable()
        {
            return dashAvailable;
        }

        public void SetDashAvailable(bool available)
        {
            dashAvailable = available;
        }
    }
}
