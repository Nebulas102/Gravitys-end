using System.Linq;
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

        private GameObject _equippedSword;

        private Item _item;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            _equippedSword = FindWeapon(player.transform);
        }

        public static GameObject FindWeapon(Transform root)
        {
            return root.Find("EquippedSword") is not null
                ? root.Find("EquippedSword").gameObject
                : (from Transform child in root select FindWeapon(child)).FirstOrDefault(result => result != null);
        }

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
                if (_item.type == Type.WEAPON)
                    _equippedSword.SetActive(false);

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

            _item.Use(this, _equippedSword);
        }
    }
}
