using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField]
        public GameObject iconDisplay;

        [SerializeField]
        public GameObject removeButton;

        [SerializeField]
        public bool isEquippedSlot;

        public GameObject item { get; private set; }

        public void SetItem(Item obj)
        {
            ToggleItem(obj.gameObject, obj.icon);
        }

        public void DropItem()
        {
            ToggleItem(null, null);

            // TODO: Instantiate the item on the ground
        }

        private void ToggleItem(GameObject obj, Sprite sprite)
        {
            // Get the Image and Button components of the icon display and remove button
            var iconImage = iconDisplay.GetComponent<Image>();
            var iconButton = iconDisplay.GetComponent<Button>();
            var removeImage = removeButton.GetComponent<Image>();
            var removeBtn = removeButton.GetComponent<Button>();

            // Set the item and icon sprite
            item = obj;
            iconImage.sprite = sprite;

            // Enable or disable the icon display and remove button based on whether the slot is empty
            var isEmpty = IsEmpty();
            iconImage.enabled = !isEmpty;
            iconButton.enabled = !isEmpty;
            iconButton.interactable = !isEmpty;
            removeImage.enabled = !isEmpty;
            removeBtn.enabled = !isEmpty;
            removeBtn.interactable = !isEmpty;
        }

        public bool IsEmpty()
        {
            return item is null || item.gameObject is null;
        }

        public void Use()
        {
            InventoryManager.instance.UseItem(this);
        }
    }
}
