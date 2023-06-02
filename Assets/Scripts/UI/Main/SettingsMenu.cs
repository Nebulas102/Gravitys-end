using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Main.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        public GameObject mainMenuButton;

        void Update()
        {
            EventSystem.current.SetSelectedGameObject(mainMenuButton);
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
