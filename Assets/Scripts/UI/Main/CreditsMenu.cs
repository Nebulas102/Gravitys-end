using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI.Main
{
    public class CreditsMenu : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }
    }
}
