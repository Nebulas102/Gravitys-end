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
        private bool moveTriggered;
        private Animator animator;
        private const float ComboDelay = 0.6f;


        private Character _player = PlayerManager.Instance.player.GetComponent<Character>();
        private Vector2 mousePos;

        public Camera _camera;
        private PlayerInput playerInput;


        public AttackState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
            animator = PlayerAnimator.Instance._animator;
            playerInput = _player.playerInput;

        }

        public override void Enter()
        {
            base.Enter();
            attackTriggered = false;
            moveTriggered = false;
            timePassed = 0f;

            //todo need to check if keyboard or gamepad
            if (_player.playerInput.currentControlScheme != "Gamepad")
            {
                _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                mousePos = lookAction.ReadValue<Vector2>();
                Ray ray = _camera.ScreenPointToRay(mousePos);
                float rayDistance = Mathf.Infinity;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, rayDistance, _player.allowedLayers, QueryTriggerInteraction.Ignore))
                {
                    Vector3 pointToLook = raycastHit.collider.gameObject.CompareTag("Enemy") || raycastHit.collider.gameObject.CompareTag("Boss")
                        ? new Vector3(raycastHit.collider.gameObject.transform.position.x, _player.transform.position.y, raycastHit.collider.gameObject.transform.position.z)
                        : raycastHit.point;

                    pointToLook.y = _player.transform.position.y;
                    _player.lookAtPosition = pointToLook;
                    _player.transform.LookAt(_player.lookAtPosition);
                }
            }


            if (IsMelee())
            {
                animator.SetTrigger("attack");
            }

            if (IsRanged())
            {
                velocity = Vector3.zero;
                input = Vector2.zero;
                PlayerAnimator.Instance._animator.SetFloat("Velocity", 0);
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
            timePassed += Time.deltaTime;

            bool isMelee = IsMelee();
            bool isRanged = IsRanged();

            if ((isMelee && timePassed < ComboDelay && attackTriggered) || (isRanged && attackTriggered))
            {
                timePassed = 0f;
                stateMachine.ChangeState(character.attacking);
            }

            float clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            float clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
            float comboLength = clipLength / clipSpeed;

            if ((isMelee && timePassed >= comboLength) || (isRanged && timePassed >= 0.4f))
            {
                stateMachine.ChangeState(character.combatting);
                animator.SetTrigger("move");
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private bool IsMelee()
        {
            return EquipmentSystem.Instance._equippedWeapon != null && EquipmentSystem.Instance._equippedWeapon.CompareTag("Melee");
        }

        private bool IsRanged()
        {
            return EquipmentSystem.Instance._equippedWeapon != null && EquipmentSystem.Instance._equippedWeapon.CompareTag("Ranged");
        }
    }
}