using UnityEngine;
using UnityEngine.EventSystems;
using UI.Runtime;
using UI.Inventory;
using UnityEngine.InputSystem;

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
        private GameObject itemPickupPromptKeyboard;

        [SerializeField]
        private GameObject itemPickupPromptGamepad;

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

        private void OnShowPickupPrompt(bool show)
        {
            bool isGamepadUsed = Gamepad.current != null && Gamepad.current.leftStick.ReadValue().magnitude > 0.1f;

            if (isGamepadUsed)
            {
                // Gamepad is being used, use the gamepad prompt
                if (itemPickupPromptKeyboard.activeSelf) 
                    itemPickupPromptKeyboard.SetActive(false);

                itemPickupPromptGamepad.SetActive(show);
            }
            else
            {
                // Keyboard is being used, use the keyboard prompt
                if (itemPickupPromptGamepad.activeSelf) 
                    itemPickupPromptGamepad.SetActive(false);

                itemPickupPromptKeyboard.SetActive(show);
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
