using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        public GameObject pauseMenu;

        public void Resume()
        {
            // This removes the pause menu overlay to continue the game
            pauseMenu.SetActive(false);
            // Unpause the game
            // 1f for now, Depends in which scene we are and what the timescale is in that particular scene
            Time.timeScale = 1f;
            // Maybe set the boolean isPaused on false here if there is a boolean
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
