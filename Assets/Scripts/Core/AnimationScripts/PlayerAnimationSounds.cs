using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSounds : MonoBehaviour
{

    AudioSource animationSoundPlayer;

    [SerializeField]
    private AudioClip[] animationSounds;
    // Start is called before the first frame update
    void Start()
    {
        animationSoundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // first element in animationSounds is footstep sound
    private void PlayerFootstepSound() 
    {
        animationSoundPlayer.clip = animationSounds[0];
        animationSoundPlayer.Play();
    }

    // second element in animationSounds is shoot sound
    private void PlayerShootSound() 
    {
        animationSoundPlayer.clip = animationSounds[1];
        animationSoundPlayer.Play();
    }
}
