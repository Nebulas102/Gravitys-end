using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class CreditsMenu : MonoBehaviour
    {

        void Update () {
            AutoScrollCreditsMenu();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        public void AutoScrollCreditsMenu() {

        }

    }
}
