using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenuButton;

    void Update() {
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
