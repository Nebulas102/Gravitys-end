using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class CreditsMenu : MonoBehaviour
    {
        public void GoToMainMenu2()
        {
            Debug.Log("komt hiering main menu");
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
