using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        private GameInput gameInput;

        private void Awake()
        {
            gameInput = FindObjectOfType<GameInput>();
        }

        void FixedUpdate()
        {
            
        }

        void Update()
        {

        }

        void HandleAttack()
        {
            if (gameInput.GetAttack())
            {
                Debug.Log("Attack");
            }
        }
    }
}