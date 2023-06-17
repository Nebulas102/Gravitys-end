using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

namespace Main.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuButton;
        [SerializeField] private Toggle vsyncToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown qualityDropdown;

        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;

        private float currentRefreshRate;
        private int currentResolutionIndex = 0;

        private void Start()
        {
            // Load the previous value of VSync from PlayerPrefs and set the toggle accordingly
            vsyncToggle.isOn = PlayerPrefs.GetInt("VSyncEnabled", 0) == 1;

            InitResolutions();
            InitQualityLevels();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        public void OnVSyncToggle()
        {
            // Store the value of VSync in PlayerPrefs
            int vsyncValue = vsyncToggle.isOn ? 1 : 0;
            PlayerPrefs.SetInt("VSyncEnabled", vsyncValue);
            PlayerPrefs.Save();
        }

        private void InitResolutions()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();

            resolutionDropdown.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRate;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }

            List<string> options = new List<string>();

            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
                options.Add(resolutionOption);
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        private void InitQualityLevels()
        {
            string[] qualityLevels = QualitySettings.names;
            List<string> options = new List<string>(qualityLevels);

            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(options);

            // Get the saved quality level index from PlayerPrefs
            int savedQualityIndex = PlayerPrefs.GetInt("QualityLevel", -1);

            // If a valid saved index exists, set it as the default selection
            if (savedQualityIndex != -1 && savedQualityIndex < qualityLevels.Length)
            {
                qualityDropdown.value = savedQualityIndex;
            }
            else
            {
                // Otherwise, find the index of the "High" quality level and set it as the default selection
                int defaultIndex = Array.IndexOf(qualityLevels, "High");
                if (defaultIndex != -1)
                {
                    qualityDropdown.value = defaultIndex;
                }
            }

            qualityDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, true);
        }

        public void SetQualityLevel(int qualityIndex)
        {
            PlayerPrefs.SetInt("QualityLevel", qualityIndex);
            PlayerPrefs.Save();
        }

        public void SetFullscreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }
}
