using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = new PlayerInputManager();
        playerInputManager.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputManager.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        Vector2 mousePosition = playerInputManager.Player.MousePosition.ReadValue<Vector2>();
        return mousePosition;
    }

    public bool GetDash()
    {
        bool dash = playerInputManager.Player.Dash.triggered;
        return dash;
    }
}
