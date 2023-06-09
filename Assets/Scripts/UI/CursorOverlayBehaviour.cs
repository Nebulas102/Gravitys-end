using UI.Runtime;
using UnityEngine;

namespace UI
{
    public class CursorOverlayBehaviour : MonoBehaviour
    {
        public static CursorOverlayBehaviour instance;
        public bool canCursorMove;

        [SerializeField]
        public GameObject cursor;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            UIHandler.OnPauseGameToggle += OnPauseGameToggle;
        }

        private void OnPauseGameToggle(bool opened)
        {
            cursor.SetActive(canCursorMove && opened);
        }

        public void CanCursorMove(bool toggle)
        {
            canCursorMove = toggle;
            cursor.SetActive(toggle && (InventoryOverlayBehaviour.instance.inventoryOpened || PauseMenu.instance.isPaused));
        }
    }
}