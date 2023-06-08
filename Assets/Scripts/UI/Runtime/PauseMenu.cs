using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Runtime
{
    public class PauseMenu : MonoBehaviour
    {
        public delegate void PauseToggle(bool paused);
        public static event PauseToggle OnPauseToggle;

        [SerializeField]
        public GameObject pauseMenu;

        [SerializeField]
        public GameObject controlsImage;

        private InputManager _inputManager;
        private bool _isPaused;

        // Singleton for pausemenu
        public static PauseMenu instance;
        public bool isPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if (value)
                    InventoryOverlayBehaviour.instance.inventoryOpened = false;

                pauseMenu.SetActive(value);
                OnPauseToggle?.Invoke(value);
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            _inputManager = new InputManager();
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.UI.TogglePauseMenu.performed += _ => TogglePause(!isPaused);
            _inputManager.UI.CloseMenu.performed += _ => CloseMenu();
        }

        private void OnDisable()
        {
            _inputManager.Disable();
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        public void ToggleControls()
        {
            controlsImage.SetActive(!controlsImage.activeSelf);
        }

        public void TogglePause(bool force = false)
        {
            isPaused = force;
            Time.timeScale = force ? 0f : 1f;
        }

        private void CloseMenu()
        {
            if (isPaused && pauseMenu.activeSelf)
                TogglePause();
        }
    }
}
