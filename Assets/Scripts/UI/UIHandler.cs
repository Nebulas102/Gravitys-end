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

        private void OnShowPickupPrompt(bool show, int instance, ItemType type)
        {
            if (show)
            {
                if (!_itemsNearby.Contains(instance))
                    _itemsNearby.Add(instance);
            }
            else
                _itemsNearby.Remove(instance);

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

            itemPickupPrompt.SetActive(_itemsNearby.Count > 0);
        }
    }
}
