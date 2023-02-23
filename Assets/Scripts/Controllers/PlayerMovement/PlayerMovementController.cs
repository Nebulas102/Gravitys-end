using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    public float speed;

    private Vector2 move;
    private Rigidbody rb;

    [SerializeField]
    private new Camera camera;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        movement = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * movement;
        //rotate character to face movement direction
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        //move character to new position with set speed
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
