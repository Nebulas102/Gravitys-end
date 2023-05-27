using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace UI
{
    public class GamepadCursor : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;

        [SerializeField]
        private RectTransform cursor;

        [SerializeField]
        private float cursorSpeed = 1000f;

        [SerializeField]
        private RectTransform canvasRectTransform;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private float padding = 25f;

        private bool previousMouseState;
        private Mouse virtualMouse;
        private Camera mainCamera;
        private Mouse currentMouse;
        private string previousControlScheme = string.Empty;

        private void OnEnable()
        {
            mainCamera = Camera.main;
            currentMouse = Mouse.current;

            if (virtualMouse is null)
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            else if (!virtualMouse.added)
                InputSystem.AddDevice(virtualMouse);

            InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            if (cursor is not null)
            {
                var position = cursor.anchoredPosition;
                InputState.Change(virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += UpdateMotion;
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            if (virtualMouse is not null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);

            InputSystem.onAfterUpdate -= UpdateMotion;
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void UpdateMotion()
        {
            if (virtualMouse is null || Gamepad.current is null || playerInput.currentControlScheme != Scheme.GAMEPAD_SCHEME)
                return;

            // Delta
            var deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;

            var currentPosition = virtualMouse.position.ReadValue();
            var newPosition = currentPosition + deltaValue;

            // Clamp
            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            // Update
            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            var aButtonIsPressed = Gamepad.current.aButton.IsPressed();
            if (previousMouseState != aButtonIsPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = aButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                position,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out var anchoredPosition
            );
            cursor.anchoredPosition = anchoredPosition;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            if (input.currentControlScheme == Scheme.KEYBOARD_MOUSE_SCHEME && previousControlScheme != Scheme.KEYBOARD_MOUSE_SCHEME)
            {
                cursor.gameObject.SetActive(false);
                Cursor.visible = true;
                currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
                previousControlScheme = Scheme.KEYBOARD_MOUSE_SCHEME;
            }
            else if (playerInput.currentControlScheme == Scheme.GAMEPAD_SCHEME && previousControlScheme != Scheme.GAMEPAD_SCHEME)
            {
                cursor.gameObject.SetActive(true);
                Cursor.visible = false;
                InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
                AnchorCursor(currentMouse.position.ReadValue());
                previousControlScheme = Scheme.GAMEPAD_SCHEME;
            }
        }

        private void Update()
        {
            if (previousControlScheme != playerInput.currentControlScheme)
                OnControlsChanged(playerInput);
            previousControlScheme = playerInput.currentControlScheme;
        }
    }

    public class Scheme
    {
        public const string GAMEPAD_SCHEME = "Gamepad";
        public const string KEYBOARD_MOUSE_SCHEME = "KeyboardMouse";
    }
}