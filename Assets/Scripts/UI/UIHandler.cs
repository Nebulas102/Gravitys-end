using UnityEngine;
using UnityEngine.EventSystems;
using UI.Runtime;
using UI.Inventory;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public delegate void PauseGameToggle(bool gamePaused);
        public static event PauseGameToggle OnPauseGameToggle;

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
            MapUIManager.OnMapToggled += OnMapToggle;
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
            OnPauseGameToggle.Invoke(inventoryOpened);
        }

        private void OnPauseToggle(bool paused)
        {
            if (paused)
            {
                // m_EventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
                currentMenuState = UIState.PAUSE;
            }
            else
            {
                m_EventSystem.SetSelectedGameObject(null);
                currentMenuState = UIState.NONE;
            }
            OnPauseGameToggle.Invoke(paused);
        }

        private void OnMapToggle(bool mapActive)
        {
            if(mapActive)
            {
                currentMenuState = UIState.MAP;
            }
            else
            {
                currentMenuState = UIState.NONE;
            }
            OnPauseGameToggle.Invoke(mapActive);
        }

        private void OnShowPickupPrompt(bool show)
        {
            itemPickupPrompt.SetActive(!itemPickupPrompt.activeSelf);
        }

        private enum UIState
        {
            NONE,
            INVENTORY,
            PAUSE,
            MAP
        }
    }
}
