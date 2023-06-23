using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UnityEngine;


public class PlayerWeaponSlash : MonoBehaviour
{
    public ParticleSystem[] attacks;

    public void PlaySlash(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < attacks.Length && !attacks[attackIndex].isPlaying)
        {
            attacks[attackIndex].Play();
        }
    }
}
