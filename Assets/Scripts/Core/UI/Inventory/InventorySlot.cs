using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField]
        public bool isCurrentSlot;

        [SerializeField]
        public Image icon;

        [SerializeField]
        public Button removeButton;

        private Item _item;


        public void AddItem(Item newItem)
        {
            // item and its contents are to be initialized
            _item = newItem;
            icon.sprite = _item.icon;
            icon.enabled = true;

            removeButton.interactable = true;
        }

        public void ClearSlot()
        {
            // item and its contents are set to null
            _item = null;
            icon.sprite = null;
            icon.enabled = false;

            removeButton.interactable = false;
        }

        public Item GetItem()
        {
            return _item != null ? _item : null;
        }

        public void OnRemoveButton()
        {
            // Something for later: Drop the item on the ground

            // If the current slot calls remove then clear that slot and get out of the function 
            // without removing the item from the array in inventory
            if (isCurrentSlot)
            {
                ClearSlot();
                return;
            }

            Inventory.Instance.Remove(_item);
        }

        public void UseItem()
        {
            // When the slot gets clicked and the slot isn't empty
            // Then you can use that particular item
            if (_item == null) return;
            _item.Use(this);
        }
    }
}
