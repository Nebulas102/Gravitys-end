using System.Collections;
using Core.UI.Inventory;
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

        // Start is called before the first frame update
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _gameInput = FindObjectOfType<GameInput>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!isDashing) HandleInput();
        }

        private void FixedUpdate()
        {
            if (Inventory.Instance.inventoryOpened) {
                return;
            }
            if (!isDashing) HandleDash();
        }

        private void HandleInput()
        {
            _movementInput = _gameInput.GetMovement();
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

        public float GetDashTimer()
        {
            return dashTimer;
        }

        public bool GetDashAvailable()
        {
            return dashAvailable;
        }

        public void SetDashAvailable(bool available) {
            dashAvailable = available;
        }
    }
}
