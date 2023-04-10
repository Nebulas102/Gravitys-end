using UnityEngine;

namespace Assets.Scripts
{
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
            Vector2 inputVector = playerInputManager.Player.Move.ReadValue<Vector2>();
            return inputVector;
        }

        public Vector2 GetLookPosition()
        {
            Vector2 lookPosition = playerInputManager.Player.Look.ReadValue<Vector2>();
            return lookPosition;
        }

        public bool GetDash()
        {
            bool dash = playerInputManager.Player.Dash.ReadValue<float>() > 0;
            return dash;
        }

        public bool GetAttack()
        {
            bool attack = playerInputManager.Player.Attack.triggered;
            return attack;
        }
    }
}
