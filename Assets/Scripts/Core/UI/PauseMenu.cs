using UnityEngine;
using UnityEngine.SceneManagement;
using Core.UI.Inventory;
using UI;

namespace Core.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        public GameObject pauseMenu;

        private UIMenus _UIMenus;
        private bool pauseMenuToggleInput;
        private bool closeMenuInput;

        // Singleton for inventory
        public static PauseMenu instance;
        public bool isPaused;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            _UIMenus = new UIMenus();
            isPaused = false;
        }

        private void Update()
        {
            if (pauseMenu.activeSelf)
            {
                //This doesn't work, the game doesn't use Time.DeltaTime for enemy behaviour and such so nothing actually stops.
                Time.timeScale = 0f;
                OverlayBehaviour.instance.inventoryOpened = false;
            }
            else
            {
                Time.timeScale = 1f;
            }

            // Toggle pause menu
            if (pauseMenuToggleInput)
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                isPaused = pauseMenu.activeSelf;
            }

            if (closeMenuInput) {
                if (isPaused) {
                    Resume();
                }
            }

            pauseMenuToggleInput = false;
            closeMenuInput = false;
        }

        private void OnEnable()
        {
            _UIMenus.Enable();
            _UIMenus.Menus.TogglePauseMenu.performed += ctx => pauseMenuToggleInput = true;
            _UIMenus.Menus.CloseMenu.performed += ctx => closeMenuInput = true;
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
            // 1f for now, depends in which scene we are and what the timescale is in that particular scene
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
