using Core.UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputManager))]
    public class Character : MonoBehaviour
    {
        [Header("Controls")]
        public float playerSpeed = 5.0f;

        [HideInInspector]
        public CharacterController controller;

        [HideInInspector]
        public PlayerInput playerInput;

        [HideInInspector]
        public Camera camera;

        [HideInInspector]
        public Vector3 lookAtPosition;

        [HideInInspector]
        public Animator animator;

        [HideInInspector]
        public Vector3 playerVelocity;


        public StateMachine movementSM;
        public SprintState sprinting;
        public StandingState standing;
        public CombatState combatting;
        public AttackState attacking;

        private GameObject _player;


        // Start is called before the first frame update
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
            camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

            movementSM = new StateMachine();
            standing = new StandingState(this, movementSM);
            sprinting = new SprintState(this, movementSM);
            combatting = new CombatState(this, movementSM);
            attacking = new AttackState(this, movementSM);

            movementSM.Initialize(standing);

            _player = PlayerManager.Instance.player;
        }

        private void Update()
        {
            if (Inventory.Instance.inventoryOpened || DialogueManager.Instance.dialogueActive) {
                movementSM.ChangeState(standing);
            } else {
                movementSM.currentState.HandleInput();
                movementSM.currentState.LogicUpdate();
            }
        }

        private void FixedUpdate()
        {
            //Look at mouse
            Vector2 mousePosition = playerInput.actions["Look"].ReadValue<Vector2>();

            Ray ray = camera.ScreenPointToRay(mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 pointToLook = ray.GetPoint(rayDistance);

                lookAtPosition = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);

                _player.transform.LookAt(lookAtPosition);
            }

            movementSM.currentState.PhysicsUpdate();
        }
    }
}
