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

    public Vector2 GetLookPosition()
    {
        Vector2 lookPosition = playerInputManager.Player.Look.ReadValue<Vector2>();
        return lookPosition;
    }


    public bool GetDash()
    {
        bool dash = playerInputManager.Player.Dash.triggered;
        return dash;
    }
}
