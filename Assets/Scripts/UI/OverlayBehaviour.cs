using UnityEngine;

namespace UI
{
    public class OverlayBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject overlay;

        private void OnToggleInventory()
        {
            overlay.SetActive(!overlay.activeSelf);
        }
    }
}
