using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    private Camera _camera;
    private Image cursorSprite;
    private bool _isUsingGamepad = false;

    void Start()
    {
        _camera = Camera.main;
        cursorSprite = GetComponent<Image>();
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            // Check if the gamepad joystick is being used
            Vector2 joystick = Gamepad.current.leftStick.ReadValue();
            if (joystick.magnitude > 0.0f)
            {
                _isUsingGamepad = true;
                // Move the cursor sprite based on the joystick input
                transform.position += new Vector3(joystick.x, joystick.y, 0.0f);

                // Limit the cursor position within the screen bounds
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0.0f, Screen.width);
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0.0f, Screen.height);
                transform.position = clampedPosition;

                Debug.Log(1);

                // Check if the cursor is hovering over a button
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = transform.position;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    // If a button is found, select it
                    Button button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.Select();
                        break;
                    }
                }
            }
            // Check if the gamepad button is being pressed
            if (Gamepad.current.aButton.isPressed)
            {
                // Simulate a mouse click
                Debug.Log("Click");
            }
        }
        // Check if mouse is being used
        if (Mouse.current != null && Mouse.current.delta.ReadValue().magnitude > 0.0f)
        {
            _isUsingGamepad = false;
            // Set the cursor to the system cursor
            Cursor.visible = true;
            cursorSprite.enabled = false;
            // Move the cursor sprite to the mouse position
            transform.position = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Debug.Log(2);

            // Check if the cursor is hovering over a button
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = transform.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                // If a button is found, select it
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    button.Select();
                    break;
                }
            }
        }
        else
        {
            // Set the cursor to the game cursor
            cursorSprite.enabled = _isUsingGamepad;
            Cursor.visible = !_isUsingGamepad;
        }
    }
}