using UnityEngine;
using UI.Runtime;
using UI.Inventory;

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

        private void Start()
        {
            InventoryOverlayBehaviour.OnInventoryToggle += (bool active) => PauseGame(active);
            PauseMenu.OnPauseToggle += (bool active) => PauseGame(active);
            MapUIManager.OnMapToggled += (bool active) => PauseGame(active);

            Item.OnItemPickup += OnShowPickupPrompt;
        }

        private void OnDestroy()
        {
            Item.OnItemPickup -= OnShowPickupPrompt;
        }

        private void PauseGame(bool condition)
        {
            OnPauseGameToggle.Invoke(condition);
        }

        private void OnShowPickupPrompt(bool show)
        {
            itemPickupPrompt.SetActive(!itemPickupPrompt.activeSelf);
        }
    }
}
