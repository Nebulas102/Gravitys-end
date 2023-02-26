using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Vector2 move;
    private Rigidbody rb;

    private bool dashInput;
    private Vector2 wasdInput;
    private Vector2 mouseInput;

    void Awake()
    {
        //freeze rotation so player doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        wasdInput = gameInput.GetMovementVectorNormalized();
        mouseInput = gameInput.GetMousePosition();
        dashInput = gameInput.GetDash();
    }

    void FixedUpdate()
    {
        OnMove();
        OnMouse();
        OnDash();
    }

    public bool IsRunning()
    {
        return isRunning;
    }


    public Vector3 GetMovementDirection()
    {
        //gets input from player
        var inputVector = wasdInput;
        //create vector3 from input
        var movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        //makes player move independent of camera rotation (W means north, S means south, etc.)
        return Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * movementDirection;
    }


    public void OnMove()
    {
        //get movement direction
        var movementDirection = GetMovementDirection();
        //set running to true if movement direction is not zero for animation to start
        isRunning = movementDirection != Vector3.zero;
        //move character to new position with set speed
        rb.MovePosition(transform.position + movementDirection * speed * Time.deltaTime);
    }


    //rotate player to face mouse position
    private void OnMouse()
    {
        var cameraRay = camera.ScreenPointToRay(mouseInput);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            var pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
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