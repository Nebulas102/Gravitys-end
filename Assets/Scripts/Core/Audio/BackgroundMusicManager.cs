using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip normalBackgroundMusic;
    [SerializeField] private AudioClip bossBackgroundMusic;
    [SerializeField] private Slider backgroundMusicSlider;
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

        // Check if PlayerPrefs has a stored value for BackgroundMusicVolume
        if (!PlayerPrefs.HasKey("BackgroundMusicVolume"))
        {
            SetBackgroundMusicVolume(0.5f);
        }
        SetBackgroundMusicVolume(PlayerPrefs.GetFloat("BackgroundMusicVolume"));
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
        backgroundMusicSlider.value = volume;
        PlayerPrefs.SetFloat("BackgroundMusicVolume", volume);
        PlayerPrefs.Save();
    }
}
