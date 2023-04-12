using UnityEngine;

namespace Controllers.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        public static PlayerAnimator instance;

        private Animator animator;

        private string currentState;

        private void Awake()
        {
            if (instance == null) instance = this;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void ChangeAnimationState(string newState)
        {
            //stop the same animation from interrupting itself
            if (currentState == newState) return;
            //play new animation
            animator.Play(newState);
            //update current state
            currentState = newState;
        }


        public void PlayRunning()
        {
            animator.Play("Running");
        }

        public void PlayIdle()
        {
            animator.Play("Idle");
        }

        public void PlayAttack1()
        {
            animator.Play("Attack1");
        }

        public void PlayAttack2()
        {
            animator.Play("Attack2");
        }

        public void PlayAttack3()
        {
            animator.Play("Attack3");
        }
    }
}
