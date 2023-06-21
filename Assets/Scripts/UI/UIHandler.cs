using UnityEngine;
using UI.Runtime;
using UI.Inventory;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        public delegate void PauseGameToggle(bool gamePaused);
        public static event PauseGameToggle OnPauseGameToggle;

        [SerializeField]
        private GameObject inventoryMenuFirstSelected;

        [SerializeField]
        private GameObject pauseMenuFirstSelected;

        [SerializeField]
        private GameObject itemPickupPrompt;

        private List<int> _itemsNearby = new List<int>();
        private InputManager _inputManager;

        private void Start()
        {
            InventoryOverlayBehaviour.OnInventoryToggle += (bool active) => PauseGame(active);
            PauseMenu.OnPauseToggle += (bool active) => PauseGame(active);
            MapUIManager.OnMapToggled += (bool active) => PauseGame(active);

            Item.OnItemPickup += OnShowPickupPrompt;
            _inputManager = new InputManager();
        }

        private void OnDestroy()
        {
            Item.OnItemPickup -= OnShowPickupPrompt;
        }

        private void PauseGame(bool condition)
        {
            OnPauseGameToggle.Invoke(condition);
        }

        public void ShowPickupPrompt(ItemType type, bool show)
        {
            if (!show)
            {
                itemPickupPrompt.SetActive(false);
                return;
            }

            var prompt = itemPickupPrompt.GetComponent<TextMeshProUGUI>();
            if (InventoryManager.instance.IsInventoryFull(type))
                prompt.text = "Inventory full";
            else
            {
                var action = _inputManager.Player.LootPickup;
                int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
                string key = InputControlPath.ToHumanReadableString(
                    action.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );
                prompt.text = $"[{key}] Take";
            }

            itemPickupPrompt.SetActive(true);
        }

        private void OnShowPickupPrompt(bool show, ItemType type)
        {
            if (!show)
            {
                itemPickupPrompt.SetActive(false);
                return;
            }

            var prompt = itemPickupPrompt.GetComponent<TextMeshProUGUI>();
            if (InventoryManager.instance.IsInventoryFull(type))
                prompt.text = "Inventory full";
            else
            {
                var action = _inputManager.Player.LootPickup;
                int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
                string key = InputControlPath.ToHumanReadableString(
                    action.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );
                prompt.text = $"[{key}] Take";
            }

            itemPickupPrompt.SetActive(true);
        }
    }
}
