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
    private bool dashAvailble = true;

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private new Camera camera;

    //variable for running animation to start
    private bool isRunning;

    private Vector2 move;
    private Rigidbody rb;



    void Awake()
    {
        //freeze rotation so player doesn't fall over
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
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
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
    }


    //rotate player to face mouse position
    private void OnMouse()
    {
        Ray cameraRay = camera.ScreenPointToRay(gameInput.GetMousePosition());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    //get dash input and dash player in direction of input
    public void OnDash()
    {
        bool dashClicked = gameInput.GetDash();
        if (!dashAvailble && dashClicked)
        {
            Debug.Log("Dash not available");
        }
        if (dashAvailble && dashClicked)
        {
            dashAvailble = false;
            transform.Translate(GetMovementDirection() * dashDistance, Space.World);
            //start independent cooldown
            StartCoroutine(DashCooldown());

        }

    }

    //dash cooldown
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashAvailble = true;
        Debug.Log("DASH READY");
    }

}