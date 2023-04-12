using UnityEngine;

public class GameInput : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = new PlayerInputManager();
        playerInputManager.Player.Enable();
    }

    public Vector2 GetMovement()
    {
        var inputVector = playerInputManager.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetLookPosition()
    {
        var lookPosition = playerInputManager.Player.Look.ReadValue<Vector2>();
        return lookPosition;
    }

    public bool GetDash()
    {
        var dash = playerInputManager.Player.Dash.ReadValue<float>() > 0;
        return dash;
    }

    public bool GetAttack()
    {
        var attack = playerInputManager.Player.Attack.triggered;
        return attack;
    }
}
