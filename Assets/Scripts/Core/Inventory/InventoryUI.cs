using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        public Transform itemsParent;
        
        [SerializeField]
        private InventorySlot[] armorSlots;

        [SerializeField]
        private InventorySlot[] weaponSlots;

        [SerializeField]
        private InventorySlot currentArmorSlot;

        [SerializeField]
        private InventorySlot currentWeaponSlot;

        public static InventorySlot staticCurrentArmorSlot;
        public static InventorySlot staticCurrentWeaponSlot;
        public Inventory inventory;
        public InventorySlot[] slots;
        
        // Singleton for inventoryUI
        public static InventoryUI instance;

        private void Awake()
        {
            if (instance != null)
                return;

            instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Get the singleton instance of the inventory and subscribe to the delegate
            inventory = Inventory.instance;
            inventory.onItemChangedCallback += UpdateGUI;

            // Get all the inventory slots
            slots = itemsParent.GetComponentsInChildren<InventorySlot>();

            // Set the current slots to the static one's in order to obtain the right ones in other scripts
            staticCurrentArmorSlot = currentArmorSlot;
            staticCurrentWeaponSlot = currentWeaponSlot;

            UpdateGUI();
        }


        private void UpdateGUI()
        {
            //TODO: Dit hieronder werkt niet
            // HandleSlots(armorSlots, inventory.armorItems);
            // HandleSlots(weaponSlots, inventory.weaponItems);

            // Loop through all slots when the itemchanged delegate is invoked. So the inventory GUI gets updated
            for (int i = 0; i < armorSlots.Length; i++)
            {
                List<Item> armorItems = inventory.items.First(x => x.Key == Type.ARMOR).Value;
                // If an item is to be added
                if (i < armorItems.Count)
                {
                    // Add it
                    armorSlots[i].AddItem(armorItems[i]);
                }
                else
                {
                    // If not then clear the slot
                    armorSlots[i].ClearSlot();
                }

            }

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                List<Item> weaponItems = inventory.items.First(x => x.Key == Type.WEAPON).Value;
                // If an item is to be added
                if (i < weaponItems.Count)
                {
                    // Add it
                    weaponSlots[i].AddItem(weaponItems[i]);
                }
                else
                {
                    // If not then clear the slot
                    weaponSlots[i].ClearSlot();
                }

            }
        }

        // private void HandleSlots(InventorySlot[] slots, List<Item> items)
        // {
        //     for (int i = 0; i < slots.Length; i++)
        //     {
        //         // If an item is to be added
        //         if (i < inventory.items.Count)
        //         {
        //             // Add it
        //             slots[i].AddItem(inventory.items[i]);
        //         }
        //         else
        //         {
        //             // If not then clear the slot
        //             slots[i].ClearSlot();
        //         }
        //
        //     }
        // }


        public InventorySlot CheckForAvailableSlots(Type type)
        {
            if (type == Type.ARMOR)
            {
                List<Item> armorItems = inventory.items.First(x => x.Key == Type.ARMOR).Value;
                for (int i = 0; i < armorSlots.Length; i++)
                {
                    if (!(i < armorItems.Count))
                    {
                        return armorSlots[i];
                    }


                }

            }

            if (type == Type.WEAPON)
            {
                List<Item> weaponItems = inventory.items.First(x => x.Key == Type.WEAPON).Value;
                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (!(i < weaponItems.Count))
                    {
                        return weaponSlots[i];
                    }


                }

            }

            return null;
        }

        // InventorySlot SlotCheckingByType (Type type) {
        //     if (type == Type.ARMOR)
        //     {
        //         for (int i = 0; i < armorSlots.Length; i++)
        //         {
        //             if (!(i < inventory.armorItems.Count))
        //             {
        //                 return armorSlots[i];
        //             }


        //         }

        //     }
        // }
    }
}
