using UnityEngine;

namespace Controllers.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        public static PlayerAnimator Instance;

        private Animator _animator;

        private string _currentState;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void ChangeAnimationState(string newState)
        {
            //stop the same animation from interrupting itself
            if (_currentState == newState) return;
            //play new animation
            _animator.Play(newState);
            //update current state
            _currentState = newState;
        }


        public void PlayRunning()
        {
            _animator.Play("Running");
        }

        public void PlayIdle()
        {
            _animator.Play("Idle");
        }

        public void PlayAttack1()
        {
            _animator.Play("Attack1");
        }

        public void PlayAttack2()
        {
            _animator.Play("Attack2");
        }

        public void PlayAttack3()
        {
            _animator.Play("Attack3");
        }
    }
}
