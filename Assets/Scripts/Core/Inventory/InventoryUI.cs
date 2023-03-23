using System.Collections;
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
        private List<InventorySlot> armorSlots;

        [SerializeField]
        private List<InventorySlot> weaponSlots;

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
            UpdateSlots(armorSlots, Type.ARMOR);
            UpdateSlots(weaponSlots, Type.WEAPON);
        }

        private void UpdateSlots(List<InventorySlot> inventorySlots, Type itemType)
        {
            List<Item> items = inventory.items[itemType];

            foreach (InventorySlot slot in inventorySlots)
            {
                int index = inventorySlots.IndexOf(slot);

                if (index < items.Count)
                {
                    slot.AddItem(items[index]);
                }
                else
                {
                    slot.ClearSlot();
                }
            }
        }


        public InventorySlot CheckForAvailableSlots(Type type)
        {
            InventorySlot GetFirstAvailableSlot(IReadOnlyList<InventorySlot> inventorySlots, ICollection items)
            {
                return items.Count < inventorySlots.Count ? inventorySlots[items.Count] : null;
            }

            return type switch
            {
                Type.ARMOR => GetFirstAvailableSlot(armorSlots, inventory.items.First(x => x.Key == Type.ARMOR).Value),
                Type.WEAPON => GetFirstAvailableSlot(weaponSlots,
                    inventory.items.First(x => x.Key == Type.WEAPON).Value),
                _ => null
            };
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
