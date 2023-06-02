using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip normalBackgroundMusic;
    [SerializeField] private AudioClip bossBackgroundMusic;
    private static AudioClip _bossBackgroundMusic;
    public static AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = normalBackgroundMusic;
            audioSource.Play();
        }
        _bossBackgroundMusic = bossBackgroundMusic;
    }

    public static void SwitchToBossBackgroundMusic()
    {
        if (audioSource != null && audioSource.clip != _bossBackgroundMusic)
        {
            audioSource.Stop();
            audioSource.clip = _bossBackgroundMusic;
            audioSource.Play();
        }
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
