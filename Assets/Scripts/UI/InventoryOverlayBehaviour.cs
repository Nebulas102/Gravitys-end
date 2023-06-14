using UI.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class InventoryOverlayBehaviour : MonoBehaviour
    {
        public delegate void InventoryToggle(bool inventoryOpened);
        public static event InventoryToggle OnInventoryToggle;

        [SerializeField]
        private GameObject overlay;

        [SerializeField]
        private GameObject entryButton;

        private InputManager _inputManager;
        private bool _inventoryOpened;
        private EventSystem _eventSystem;

        public bool inventoryOpened
        {
            get { return _inventoryOpened; }
            set
            {
                _inventoryOpened = value;
                if (value)
                    MapUIManager.instance.mapIsActive = false;
                OnInventoryToggle?.Invoke(value);
                overlay.SetActive(inventoryOpened);
                _eventSystem.SetSelectedGameObject(value ? entryButton : null);
            }
        }

        public static InventoryOverlayBehaviour instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            _inputManager = new InputManager();
            _eventSystem = EventSystem.current;
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.UI.ToggleInventory.performed += _ => OnToggleInventory();
        }

        private void OnToggleInventory()
        {
            if (PauseMenu.instance.isPaused) return;

            inventoryOpened = !overlay.activeSelf;
            Time.timeScale = inventoryOpened ? 0f : 1f;
        }
    }
}
