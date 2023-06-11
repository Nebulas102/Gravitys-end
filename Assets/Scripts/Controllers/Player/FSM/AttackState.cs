using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Player
{
    public class AttackState : State
    {
        private float timePassed;
        private bool attackTriggered;
        private Animator animator;
        private const float ComboDelay = 0.6f;


        private Character _player = PlayerManager.Instance.player.GetComponent<Character>();
        private Vector2 mousePos;
        private Vector3 lookAtPosition;

        public Camera _camera;
        private PlayerInput playerInput;
        private bool moveTriggered;


        public AttackState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
            animator = PlayerAnimator.Instance._animator;
        }

        public override void Enter()
        {
            base.Enter();
            attackTriggered = false;
            timePassed = 0f;

            //todo need to check if keyboard or gamepad
            if (_player.playerInput.currentControlScheme != "Gamepad")
            {
                _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                mousePos = lookAction.ReadValue<Vector2>();
                Ray ray = _camera.ScreenPointToRay(mousePos);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float rayDistance;

                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 pointToLook = ray.GetPoint(rayDistance);

                    lookAtPosition = new Vector3(pointToLook.x, _player.transform.position.y, pointToLook.z);

                    _player.transform.LookAt(lookAtPosition);
                }

            }
            if (EquipmentSystem.Instance._equippedWeapon.CompareTag("Melee"))
            {
                animator.SetTrigger("attack");
            }

            if (EquipmentSystem.Instance._equippedWeapon.CompareTag("Ranged"))
            {
                animator.SetTrigger("shoot");
            }
        }

        public override void HandleInput()
        {
            base.HandleInput();

            // Handle input for attack action
            if (attackAction.triggered)
            {
                attackTriggered = true;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (EquipmentSystem.Instance._equippedWeapon.CompareTag("Melee"))
            {
                timePassed += Time.deltaTime;

                // Calculate combo length based on the clip length and speed
                float clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                float clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
                float comboLength = clipLength / clipSpeed;

                // Check if attack triggered during combo delay
                if (timePassed < ComboDelay && attackTriggered)
                {
                    timePassed = 0f;
                    stateMachine.ChangeState(character.attacking);
                }

                // Check if combo length has passed
                if (timePassed >= comboLength)
                {
                    animator.SetTrigger("move");
                    stateMachine.ChangeState(character.combatting);
                }
            }

            if (EquipmentSystem.Instance._equippedWeapon.CompareTag("Ranged"))
            {
                if (attackTriggered)
                {
                    animator.SetTrigger("shoot");

                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}