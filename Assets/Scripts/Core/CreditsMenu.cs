using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class CreditsMenu : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
