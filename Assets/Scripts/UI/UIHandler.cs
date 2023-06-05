using UnityEngine;
using UnityEngine.EventSystems;
using UI.Runtime;
using UI.Inventory;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        EventSystem m_EventSystem;

        [SerializeField]
        private GameObject inventoryMenuFirstSelected;

        [SerializeField]
        private GameObject pauseMenuFirstSelected;

        [SerializeField]
        private GameObject itemPickupPrompt;

        private UIState currentMenuState = UIState.NONE;

        private void Start()
        {
            InventoryOverlayBehaviour.OnInventoryToggle += OnInventoryToggle;
            PauseMenu.OnPauseToggle += OnPauseToggle;
            Item.OnItemPickup += OnShowPickupPrompt;
        }

        private void OnEnable()
        {
            m_EventSystem = EventSystem.current;
        }

        private void OnDisable()
        {
            m_EventSystem.SetSelectedGameObject(null);
            currentMenuState = UIState.NONE;
        }

        private void OnDestroy()
        {
            Item.OnItemPickup -= OnShowPickupPrompt;
        }

        private void OnInventoryToggle(bool inventoryOpened)
        {
            if (inventoryOpened)
            {
                // m_EventSystem.SetSelectedGameObject(inventoryMenuFirstSelected);
                currentMenuState = UIState.INVENTORY;
            }
            else
            {
                m_EventSystem.SetSelectedGameObject(null);
                currentMenuState = UIState.NONE;
            }
        }

        private void OnPauseToggle(bool paused)
        {
            if (paused)
            {
                m_EventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
                currentMenuState = UIState.PAUSE;
            }
            else
            {
                m_EventSystem.SetSelectedGameObject(null);
                currentMenuState = UIState.NONE;
            }
        }

        private void OnShowPickupPrompt(bool show)
        {
            itemPickupPrompt.SetActive(!itemPickupPrompt.activeSelf);
        }

        private enum UIState
        {
            NONE,
            INVENTORY,
            PAUSE
        }
    }
}
