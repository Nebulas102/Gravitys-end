using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;

    // The audiosource where the soundeffects will be playing from
    private AudioSource soundEffectSource;

    [SerializeField] private Slider soundEffectsSlider;

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
    [SerializeField] private AudioClip bossYells;
    [SerializeField] private AudioClip playerTakesDamage;
    [SerializeField] private AudioClip armorPickup;
    [SerializeField] private AudioClip clockTicking;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        soundEffectSource = GetComponent<AudioSource>();

        // Check if PlayerPrefs has a stored value for SoundEffectsVolume
        if (!PlayerPrefs.HasKey("SoundEffectsVolume"))
        {
            SetSoundEffectsVolume(0.5f);
        }
        SetSoundEffectsVolume(PlayerPrefs.GetFloat("SoundEffectsVolume"));
    }

    private void Start()
    {
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
        soundEffects.Add(SoundEffect.BossShoots, bossShoots);
        soundEffects.Add(SoundEffect.BossDies, bossDies);
        soundEffects.Add(SoundEffect.Dash, dashingSound);
        soundEffects.Add(SoundEffect.BossClockShot, bossClockShot);
        soundEffects.Add(SoundEffect.BossYells, bossYells);
        soundEffects.Add(SoundEffect.PlayerTakeDamage, playerTakesDamage);
        soundEffects.Add(SoundEffect.ArmorPickup, armorPickup);
        soundEffects.Add(SoundEffect.ClockTicking, clockTicking);
    }

    public void PlaySoundEffect(SoundEffect soundEffectType)
    {
        if (soundEffects.ContainsKey(soundEffectType))
        {
            AudioClip soundEffect = soundEffects[soundEffectType];
            soundEffectSource.PlayOneShot(soundEffect);
        }
        else
        {
            Debug.LogError("Sound effect '" + soundEffectType + "' not found!");
        }
    }

    public void StopSoundEffect(SoundEffect soundEffectType)
    {
            soundEffectSource.Stop();
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectSource.volume = volume;
        soundEffectsSlider.value = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        PlayerPrefs.Save();
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
        BossShoots,
        BossClockShot,
        BossDies,
        Dash,
        BossYells,
        PlayerTakeDamage,
        ArmorPickup,
        ClockTicking
    }
}
