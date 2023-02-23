using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    int isRunningHash;


    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        
    }
    
    void FixedUpdate(){

        bool isRunning = animator.GetBool(isRunningHash);
        // bool forwardPressed = Input.GetKey(KeyCode.W);
        bool wasdPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        //if player presses W and is not running, start running
        if(wasdPressed && !isRunning){
            Debug.Log("wasdpressed");
            animator.SetBool(isRunningHash, true);
        }

        //if player stops pressing W and is running, stop running
        if(!wasdPressed && isRunning){
            Debug.Log("wasdpressed");
            animator.SetBool(isRunningHash, false);
        }
    }
}
