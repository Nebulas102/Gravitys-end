using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScriptCameraTesting : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private Rigidbody rb;
    [SerializeField] private float speed = 15f;


    // Dont mind the code, this script is pure to showcase the working of the camera
    // For the player movement we should use the new input system so this IS NOT the player movement script
    // This script will be deleted after the showcase

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical") * speed;
        float horizontalInput = Input.GetAxis("Horizontal") * speed;

        rb.transform.Translate(horizontalInput * Time.deltaTime, 0, verticalInput * Time.deltaTime);

 
    }
}
