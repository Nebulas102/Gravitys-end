using Core.UI;
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

        private UIMenus _uiMenus;
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

            _uiMenus = new UIMenus();
        }

        private void OnEnable()
        {
            _uiMenus.Enable();
            _uiMenus.Menus.ToggleInventory.performed += _ => OnToggleInventory();
        }

        private void OnToggleInventory()
        {
            if (PauseMenu.instance.isPaused) return;

            inventoryOpened = !overlay.activeSelf;
        }
    }
}
