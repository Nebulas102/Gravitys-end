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

        [SerializeField]
        private GameObject cursor;

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
                cursor.SetActive(inventoryOpened);
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
            _inputManager.UI.TriggerInventory.performed += _ => TriggerToggleInventory();
        }

        private void TriggerToggleInventory()
        {
            if (PauseMenu.instance.isPaused) return;

            inventoryOpened = !overlay.activeSelf;
        }
    }
}
