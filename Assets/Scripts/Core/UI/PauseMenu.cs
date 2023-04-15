using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        public GameObject pauseMenu;

        private UIMenus _UIMenus;
        private bool pauseMenuToggleInput;

        private void Awake()
        {
            _UIMenus = new UIMenus();
        }

        private void Update()
        {
            // Toggle pause menu
            if (pauseMenuToggleInput) pauseMenu.SetActive(!pauseMenu.activeSelf);

            pauseMenuToggleInput = false;
        }

        private void OnEnable()
        {
            _UIMenus.Enable();
            _UIMenus.Menus.TogglePauseMenu.performed += ctx => pauseMenuToggleInput = true;
        }

        private void OnDisable()
        {
            _UIMenus.Disable();
        }

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
