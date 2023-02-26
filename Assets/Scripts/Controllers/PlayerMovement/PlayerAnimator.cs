using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";

    [SerializeField]
    private PlayerMovementController playerMovementController;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_RUNNING, playerMovementController.IsRunning());
    }
}
