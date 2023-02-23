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
        bool forwardPressed = Input.GetKey(KeyCode.W);

        //if player presses W and is not running, start running
        if(forwardPressed && !isRunning){
            Debug.Log("W is pressed");
            animator.SetBool(isRunningHash, true);
        }

        //if player stops pressing W and is running, stop running
        if(!forwardPressed && isRunning){
            Debug.Log("W is not pressed");
            animator.SetBool(isRunningHash, false);
        }
    }
}
