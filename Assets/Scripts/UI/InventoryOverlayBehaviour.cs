using UI.Runtime;
using UnityEngine;

namespace UI
{
    public class InventoryOverlayBehaviour : MonoBehaviour
    {
        public delegate void InventoryToggle(bool inventoryOpened);
        public static event InventoryToggle OnInventoryToggle;

        [SerializeField]
        private GameObject overlay;

        private InputManager _inputManager;
        private bool _inventoryOpened;

        public bool inventoryOpened
        {
            get { return _inventoryOpened; }
            set
            {
                _inventoryOpened = value;
                OnInventoryToggle?.Invoke(value);
                overlay.SetActive(inventoryOpened);
                if (value)
                    MapUIManager.instance.mapIsActive = false;
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
