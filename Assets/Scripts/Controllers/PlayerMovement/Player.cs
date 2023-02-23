using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private LookTowardMouse lookTowardMouse;

    [SerializeField]
    private new Camera camera;


    private Vector2 move;
    private Rigidbody rb;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        //freeze rotation so player doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        LookTowardMouse();
    }

    public void MovePlayer()
    {
        //gets input from player
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //create vector3 from input
        Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        //makes player move independent of camera rotation (W means north, S means south, etc.)
        movementDirection = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;

        //walking if moveement direction is not zero
        isRunning = movementDirection != Vector3.zero;
        //move character to new position with set speed
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    private void LookTowardMouse()
    {
        Ray cameraRay = camera.ScreenPointToRay(gameInput.GetMousePosition());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}

