using Core.UI;
using UnityEngine;

namespace UI
{
    public class OverlayBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlay;
        private UIMenus _uiMenus;

        public bool inventoryOpened;

        public static OverlayBehaviour instance;

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
            overlay.SetActive(inventoryOpened);
        }
    }
}
