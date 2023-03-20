using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public Transform itemsParent;

    Inventory inventory;

    InventorySlot[] slots;

    [SerializeField] InventorySlot[] armorSlots;
    [SerializeField] InventorySlot[] weaponSlots;
    [SerializeField] InventorySlot currentArmorSlot;
    [SerializeField] InventorySlot currentWeaponSlot;

    public static InventorySlot staticCurrentArmorSlot;
    public static InventorySlot staticCurrentWeaponSlot;


    // Singleton for inventoryUI
    public static InventoryUI instance;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
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


    void UpdateGUI()
    {
        //TODO: Dit hieronder werkt niet
        // HandleSlots(armorSlots, inventory.armorItems);
        // HandleSlots(weaponSlots, inventory.weaponItems);

        // Loop through all slots when the itemchanged delegate is invoked. So the inventory GUI gets updated
        for (int i = 0; i < armorSlots.Length; i++)
        {
            // If an item is to be added
            if (i < inventory.armorItems.Count)
            {
                // Add it
                armorSlots[i].AddItem(inventory.armorItems[i]);
            }
            else
            {
                // If not then clear the slot
                armorSlots[i].ClearSlot();
            }

        }
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            // If an item is to be added
            if (i < inventory.weaponItems.Count)
            {
                // Add it
                weaponSlots[i].AddItem(inventory.weaponItems[i]);
            }
            else
            {
                // If not then clear the slot
                weaponSlots[i].ClearSlot();
            }

        }
    }

    void HandleSlots(InventorySlot[] slots, List<Item> items) {
        for (int i = 0; i < slots.Length; i++)
        {
            // If an item is to be added
            if (i < inventory.items.Count)
            {
                // Add it
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                // If not then clear the slot
                slots[i].ClearSlot();
            }

        }
    }

    
    public InventorySlot CheckForAvailableSlots(Type type)
    {
        if (type == Type.ARMOR)
        {
            for (int i = 0; i < armorSlots.Length; i++)
            {
                if (!(i < inventory.armorItems.Count))
                {
                    return armorSlots[i];
                }


            }

        }
        if (type == Type.WEAPON)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (!(i < inventory.weaponItems.Count))
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
