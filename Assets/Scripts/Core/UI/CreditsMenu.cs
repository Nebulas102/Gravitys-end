using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class CreditsMenu : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
