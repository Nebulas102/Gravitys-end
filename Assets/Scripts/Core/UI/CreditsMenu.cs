using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class CreditsMenu : MonoBehaviour
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
}
