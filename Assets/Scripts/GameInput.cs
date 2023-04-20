using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = new PlayerInputManager();
        _playerInputManager.Player.Enable();
    }

    public Vector2 GetMovement()
    {
        var inputVector = _playerInputManager.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetLookPosition()
    {
        var lookPosition = _playerInputManager.Player.Look.ReadValue<Vector2>();
        return lookPosition;
    }

    public bool GetDash()
    {
        var dash = _playerInputManager.Player.Dash.ReadValue<float>() > 0;
        return dash;
    }

    public bool GetAttack()
    {
        var attack = _playerInputManager.Player.Attack.triggered;
        return attack;
    }

    public bool GetPickUp()
    {
        var pickup = _playerInputManager.Player.LootPickup.triggered;
        return pickup;
    }
}
