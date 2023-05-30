using UnityEngine;
using UnityEngine.EventSystems;
using UI;

namespace Core.UI
{
    public class UIHandler : MonoBehaviour
    {
        EventSystem m_EventSystem;


        [SerializeField]
        private GameObject inventoryMenuFirstSelected;

        [SerializeField]
        private GameObject pauseMenuFirstSelected;

        private UIState currentMenuState = UIState.NONE;

        private void Start()
        {
            InventoryOverlayBehaviour.OnInventoryToggle += OnInventoryToggle;
            PauseMenu.OnPauseToggle += OnPauseToggle;
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

        // private void Update()
        // {
        //     if (InventoryOverlayBehaviour.instance.inventoryOpened && currentMenuState != UIState.INVENTORY)
        //     {
        //         // m_EventSystem.SetSelectedGameObject(inventoryMenuFirstSelected);
        //         currentMenuState = UIState.INVENTORY;
        //     }
        //     else if (PauseMenu.instance.isPaused && currentMenuState != UIState.PAUSE)
        //     {
        //         m_EventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
        //         currentMenuState = UIState.PAUSE;
        //     }
        //     else if (!InventoryOverlayBehaviour.instance.inventoryOpened && !PauseMenu.instance.isPaused)
        //     {
        //         currentMenuState = UIState.NONE;
        //     }
        // }

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

        private enum UIState
        {
            NONE,
            INVENTORY,
            PAUSE
        }
    }


}
