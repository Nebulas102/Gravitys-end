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

        private bool _previousMouseState;
        private Mouse _virtualMouse;
        private Camera _mainCamera;
        private Mouse _currentMouse;
        private string _previousControlScheme = string.Empty;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
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
            if (_virtualMouse is null || Gamepad.current is null || playerInput.currentControlScheme != Scheme.GAMEPAD_SCHEME)
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
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
                out var anchoredPosition
            );
            cursor.anchoredPosition = anchoredPosition;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            // Switch between gamepad and keyboard/mouse control schemes
            if (input.currentControlScheme == Scheme.KEYBOARD_MOUSE_SCHEME && _previousControlScheme != Scheme.KEYBOARD_MOUSE_SCHEME)
            {
                Cursor.visible = true;
                _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                _previousControlScheme = Scheme.KEYBOARD_MOUSE_SCHEME;
            }
            else if (playerInput.currentControlScheme == Scheme.GAMEPAD_SCHEME && _previousControlScheme != Scheme.GAMEPAD_SCHEME)
            {
                Cursor.visible = false;
                InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                AnchorCursor(_currentMouse.position.ReadValue());
                _previousControlScheme = Scheme.GAMEPAD_SCHEME;
            }
        }

        private void Update()
        {
            // Check if the control scheme has changed
            if (_previousControlScheme != playerInput.currentControlScheme)
                OnControlsChanged(playerInput);
            _previousControlScheme = playerInput.currentControlScheme;
        }
    }

    public class Scheme
    {
        public const string GAMEPAD_SCHEME = "Gamepad";
        public const string KEYBOARD_MOUSE_SCHEME = "Keyboard&Mouse";
    }
}