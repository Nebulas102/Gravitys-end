using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers.PlayerMovement
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        [SerializeField]
        private float dashDistance = 2f;

        [SerializeField]
        private float dashCooldown = 5f;

        [SerializeField]
        private bool dashAvailable = true;

        [SerializeField]
        private GameInput gameInput;

        [SerializeField]
        private new Camera camera;

        //variable for running animation to start
        private bool isRunning;

        private Rigidbody rb;
        private bool dashInput;
        private Vector2 move, moveInput, lookDir, joystickLook;

        void Awake()
        {
            //freeze rotation so player doesn't fall over
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        void Update()
        {
            moveInput = gameInput.GetMovementVectorNormalized();
            lookDir = gameInput.GetLookPosition();
            dashInput = gameInput.GetDash();
        }

        void FixedUpdate()
        {
            OnMove();
            OnLook();
            OnDash();
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public Vector3 GetMovementDirection()
        {
            //gets input from player
            Vector2 inputVector = moveInput;
            //create vector3 from input
            Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
            //makes player move independent of camera rotation (W means north, S means south, etc.)
            return movementDirection = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;
        }


        public void OnMove()
        {
            //get movement direction
            Vector3 movementDirection = GetMovementDirection();
            //set running to true if movement direction is not zero for animation to start
            isRunning = movementDirection != Vector3.zero;
            //move character to new position with set speed
            rb.MovePosition(transform.position + movementDirection * speed * Time.deltaTime);
        }

        //rotate player to face mouse/joystick position
        private void OnLook()
        {
            if (lookDir == Vector2.zero)
            {
                return;
            }
            //mouse
            if (lookDir.x > 1 || lookDir.y > 1)
            {
                Ray cameraRay = camera.ScreenPointToRay(lookDir);
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                float rayLength;

                if (groundPlane.Raycast(cameraRay, out rayLength))
                {
                    Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                    transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
                }
            }
            //controller
            else if (lookDir.x <= 1 || lookDir.y <= 1 || lookDir.x <= -1 || lookDir.y <= -1)
            {
                Vector3 aimDirection = new Vector3(lookDir.x, 0, lookDir.y);

                if (aimDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 1f) * Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0);
                }
            }
        }

        //get dash input and dash player in direction of input
        public void OnDash()
        {
            if (!dashAvailable && dashInput)
            {
                Debug.Log("Dash not available");
            }

            if (dashAvailable && dashInput)
            {
                dashAvailable = false;
                rb.MovePosition(transform.position + GetMovementDirection() * dashDistance);
                //start independent cooldown
                StartCoroutine(DashCooldown());
            }
        }

        //dash cooldown
        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);
            dashAvailable = true;
            Debug.Log("DASH READY");
        }
    }
}