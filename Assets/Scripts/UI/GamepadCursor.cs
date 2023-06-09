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
        private RectTransform cursorTransform;

        [SerializeField]
        [Range(0f, 50f)]
        private float cursorSpeed = 25f;

        [SerializeField]
        private float padding = 25f;

        [Header("Canvas Settings")]
        [SerializeField]
        private RectTransform canvasRectTransform;

        [SerializeField]
        private Canvas canvas;

        private PlayerInput _playerInput;
        private Mouse virtualMouse;
        private string _previousControlScheme = string.Empty;
        private bool _previousMouseState;

        public static GamepadCursor instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            _playerInput = GetComponent<PlayerInput>();

            if (virtualMouse is null)
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            else if (!virtualMouse.added)
                InputSystem.AddDevice(virtualMouse);

            // Pair the device to the user to use the PlayerInput component with the Event System & the Virtual Mouse.
            InputUser.PerformPairingWithDevice(virtualMouse, _playerInput.user);
            if (cursorTransform is not null)
            {
                Vector2 position = cursorTransform.anchoredPosition;
                InputState.Change(virtualMouse.position, position);
            }
            InputSystem.onAfterUpdate += UpdateMotion;
        }

        private void OnDisable()
        {
            if (virtualMouse is not null && virtualMouse.added)
                InputSystem.RemoveDevice(virtualMouse);

            InputSystem.onAfterUpdate -= UpdateMotion;
        }

        private void UpdateMotion()
        {
            if (virtualMouse is null || Gamepad.current is null || !CursorOverlayBehaviour.instance.cursor.activeSelf) return;

            // Delta
            Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
            deltaValue *= cursorSpeed;

            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out Vector2 anchoredPosition);
            cursorTransform.anchoredPosition = anchoredPosition;
        }

        public void OnControlsChanged()
        {
            switch (_playerInput.currentControlScheme)
            {
                case Scheme.GAMEPAD_SCHEME:
                    CursorOverlayBehaviour.instance.CanCursorMove(true);
                    Cursor.visible = false;
                    break;
                case Scheme.KEYBOARD_MOUSE_SCHEME:
                    CursorOverlayBehaviour.instance.CanCursorMove(false);
                    Cursor.visible = true;
                    break;
            }
        }

        private void Update()
        {
            if (_previousControlScheme != _playerInput.currentControlScheme)
            {
                OnControlsChanged();
                _previousControlScheme = _playerInput.currentControlScheme;
            }
        }
    }

    public abstract class Scheme
    {
        public const string GAMEPAD_SCHEME = "Gamepad";
        public const string KEYBOARD_MOUSE_SCHEME = "Keyboard&Mouse";
    }
}
