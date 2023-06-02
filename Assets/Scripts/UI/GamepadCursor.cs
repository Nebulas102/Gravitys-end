using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace UI
{
    public class GamepadCursor : MonoBehaviour
    {
        [Header("Cursor Settings")]
        [SerializeField]
        [Tooltip("The cursor to move around the screen")]
        private RectTransform cursor;

        [SerializeField]
        [Range(0f, 5000f)]
        [Tooltip("The speed at which the cursor moves around the screen")]
        private float cursorSpeed = 1000f;

        [Header("Canvas Settings")]
        [SerializeField]
        [Tooltip("The canvas to anchor the cursor to")]
        private RectTransform canvasRectTransform;

        [SerializeField]
        [Tooltip("The canvas to anchor the cursor to")]
        private Canvas canvas;

        [SerializeField]
        [Tooltip("The padding around the screen to prevent the cursor from going off screen")]
        private float padding = 25f;

        private Mouse _currentMouse = Mouse.current;
        private string _previousControlScheme = string.Empty;
        private PlayerInput playerInput;
        private bool _previousMouseState;
        private Mouse _virtualMouse;

        private void OnEnable()
        {
            playerInput = FindObjectOfType<PlayerInput>();
            _currentMouse = Mouse.current;

            if (_virtualMouse is null)
                _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            else if (!_virtualMouse.added)
                InputSystem.AddDevice(_virtualMouse);

            InputUser.PerformPairingWithDevice(_virtualMouse, playerInput.user);

            if (cursor is not null)
            {
                var position = cursor.anchoredPosition;
                InputState.Change(_virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += UpdateMotion;
            playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            if (_virtualMouse is not null && _virtualMouse.added) InputSystem.RemoveDevice(_virtualMouse);

            InputSystem.onAfterUpdate -= UpdateMotion;
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        private void UpdateMotion()
        {
            // Check if the virtual mouse and gamepad are available and if the current control scheme is gamepad
            if (_virtualMouse is null || Gamepad.current is null ||
                playerInput.currentControlScheme != Scheme.GAMEPAD_SCHEME)
                return;

            // Get the delta value from the left stick of the gamepad and scale it by the cursor speed and delta time
            var deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;

            // Get the current position of the virtual mouse and calculate the new position
            var currentPosition = _virtualMouse.position.ReadValue();
            var newPosition = currentPosition + deltaValue;

            // Clamp the new position to the screen bounds
            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            // Update the virtual mouse position and delta
            InputState.Change(_virtualMouse.position, newPosition);
            InputState.Change(_virtualMouse.delta, deltaValue);

            // Check if the A button on the gamepad is pressed and update the virtual mouse button state accordingly
            var aButtonIsPressed = Gamepad.current.aButton.IsPressed();
            if (_previousMouseState != aButtonIsPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousMouseState = aButtonIsPressed;
            }

            // Anchor the cursor to the canvas
            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            // Anchor the cursor to the canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                position,
                null,
                out var anchoredPosition
            );
            cursor.anchoredPosition = anchoredPosition;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            switch (input.currentControlScheme)
            {
                // Switch between gamepad and keyboard/mouse control schemes
                case Scheme.KEYBOARD_MOUSE_SCHEME when _previousControlScheme != Scheme.KEYBOARD_MOUSE_SCHEME:
                    Cursor.visible = true;
                    _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                    _previousControlScheme = Scheme.KEYBOARD_MOUSE_SCHEME;
                    break;
                case Scheme.GAMEPAD_SCHEME when _previousControlScheme != Scheme.GAMEPAD_SCHEME:
                    Cursor.visible = false;
                    InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                    AnchorCursor(_currentMouse.position.ReadValue());
                    _previousControlScheme = Scheme.GAMEPAD_SCHEME;
                    break;
            }
        }
    }

    public abstract class Scheme
    {
        public const string GAMEPAD_SCHEME = "Gamepad";
        public const string KEYBOARD_MOUSE_SCHEME = "Keyboard&Mouse";
    }
}
