using System.Linq;
using Controllers.Player;
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

        private GameObject itemObject;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            // if (isCurrentSlot && itemObject != null)
            // {
            //     EquipmentSystem.Instance.SetCurrentWeapon(itemObject);
            // }
        }

        public void AddItem(GameObject newItemObject)
        {
            // item and its contents are to be initialized
            itemObject = newItemObject;
            icon.sprite = itemObject.GetComponent<BaseItem>().item.icon;
            icon.enabled = true;

            removeButton.interactable = true;
        }

        public void ClearSlot()
        {
            // item and its contents are set to null
            itemObject = null;
            icon.sprite = null;
            icon.enabled = false;

            removeButton.interactable = false;
        }

        public GameObject GetItem()
        {
            return itemObject != null ? itemObject : null;
        }

        public void OnRemoveButton()
        {
            // Something for later: Drop the item on the ground

            // If the current slot calls remove then clear that slot and get out of the function 
            // without removing the item from the array in inventory
            if (isCurrentSlot)
            {
                Item item = itemObject.GetComponent<BaseItem>().item;
                if (item.type == Type.WEAPON)
                    // _equippedSword.SetActive(false);

                ClearSlot();
                return;
            }

            Inventory.Instance.RemoveDrop(itemObject);
        }

        public void UseItem()
        {
            // When the slot gets clicked and the slot isn't empty
            // Then you can use that particular item
            if (itemObject == null) return;

            itemObject.GetComponent<BaseItem>().item.Use(this, itemObject);

            // if (isCurrentSlot)
            // {
            //     EquipmentSystem.Instance.SetCurrentWeapon(itemObject);
            // }
        }
    }
}
