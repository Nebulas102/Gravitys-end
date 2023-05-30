using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    private bool isWalkingPlaying = false;  // Flag to track if the walking sound effect is currently playing


    // The audiosource where the soundeffects will be playing from
    public AudioSource soundEffectSource;

    // Sound effects 
    private Dictionary<SoundEffect, AudioClip> soundEffects;

    [SerializeField] private AudioClip objectiveCompleted;

    [SerializeField] private AudioClip enemyShoots;
    [SerializeField] private AudioClip playerGunShotLow;
    [SerializeField] private AudioClip playerGunShotMid;
    [SerializeField] private AudioClip playerGunShotHeavy;
    [SerializeField] private AudioClip swordAttack;
    [SerializeField] private AudioClip punchingAir;
    [SerializeField] private AudioClip chestOpening;
    [SerializeField] private AudioClip gunPickup;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip bossShoots;
    [SerializeField] private AudioClip bossDies;
    [SerializeField] private AudioClip dashingSound;
    [SerializeField] private AudioClip bossClockShot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundEffectSource = GetComponent<AudioSource>();

        // Initialize the soundEffects dictionary and assign the AudioClips
        soundEffects = new Dictionary<SoundEffect, AudioClip>();

        // Adding the sound effects to the dictionary
        soundEffects.Add(SoundEffect.ObjectiveCompleted, objectiveCompleted);
        soundEffects.Add(SoundEffect.EnemyShoots, enemyShoots);
        soundEffects.Add(SoundEffect.PlayerGunShotLOW, playerGunShotLow);
        soundEffects.Add(SoundEffect.PlayerGunShotMID, playerGunShotMid);
        soundEffects.Add(SoundEffect.PlayerGunShotHEAVY, playerGunShotHeavy);
        soundEffects.Add(SoundEffect.SwordAttack, swordAttack);
        soundEffects.Add(SoundEffect.PunchingAir, punchingAir);
        soundEffects.Add(SoundEffect.ChestOpening, chestOpening);
        soundEffects.Add(SoundEffect.GunPickup, gunPickup);
        soundEffects.Add(SoundEffect.Walking, walkingSound);
        soundEffects.Add(SoundEffect.BossShoots, bossShoots);
        soundEffects.Add(SoundEffect.BossDies, bossDies);
        soundEffects.Add(SoundEffect.Dash, dashingSound);
        soundEffects.Add(SoundEffect.BossClockShot, bossClockShot);
    }

    public void PlaySoundEffect(SoundEffect soundEffectType)
    {
        if (soundEffects.ContainsKey(soundEffectType))
        {
            AudioClip soundEffect = soundEffects[soundEffectType];

            if (soundEffectType == SoundEffect.Walking && !isWalkingPlaying)
            {
                soundEffectSource.PlayOneShot(soundEffect);
                isWalkingPlaying = true;
                StartCoroutine(WaitForWalkingSoundToEnd(soundEffect.length));
            }
            else if (soundEffectType != SoundEffect.Walking)
            {
                soundEffectSource.PlayOneShot(soundEffect);
            }
        }
        else
        {
            Debug.LogError("Sound effect '" + soundEffectType + "' not found!");
        }
    }

    public void StopSoundEffect(SoundEffect soundEffectType)
    {
        if (soundEffectType == SoundEffect.Walking)
        {
            if (isWalkingPlaying)
            {
                soundEffectSource.Stop();
                isWalkingPlaying = false;
            }
        }
        // Add more cases to handle other sound effect types if needed
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectSource.volume = volume;
    }

    private System.Collections.IEnumerator WaitForWalkingSoundToEnd(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        isWalkingPlaying = false;
    }

    public enum SoundEffect
    {
        ObjectiveCompleted,
        EnemyShoots,
        PlayerGunShotLOW,
        PlayerGunShotMID,
        PlayerGunShotHEAVY,
        SwordAttack,
        PunchingAir,
        ChestOpening,
        GunPickup,
        Walking,
        BossShoots,
        BossClockShot,
        BossDies,
        Dash
    }
}
