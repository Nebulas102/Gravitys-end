using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /* 
        Add this method to the player movement controller Update() function, so when the inventory is opened the other movement actions wont be executed
        
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
    
    */

    // Singleton for inventory
    public static Inventory instance;

    // Delegate 
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    // Space for each type of item
    public int space = 3;
    public GameObject inventoryUI;
    public List<Item> items = new List<Item>();

    public List<Item> armorItems = new List<Item>();
    public List<Item> weaponItems = new List<Item>();


    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }


    void Update()
    {
        ToggleInventory();
    }


    public bool Add(Item item)
    {
        // Don't add the item if it is default, maybe it is an item which isn't supposed to be in the inventory
        if (!item.isDefaultItem)
        {
            if (item.type == Type.ARMOR)
            {
                itemAddCheck(item, armorItems);
            }
            if (item.type == Type.WEAPON)
            {
                itemAddCheck(item, weaponItems);
            }

            // If there is enough room, add the item and invoke the delegate so that the other methods can be notified
            items.Add(item);

            onItemChangedCallback?.Invoke();


            // if (item.type == Type.ARMOR)
            // {
            //     if (armorItems.Count >= space)
            //     {
            //         // Not enough room
            //         return false;
            //     }
            //     armorItems.Add(item);
            // }
            // if (item.type == Type.WEAPON)
            // {
            //     if (weaponItems.Count >= space)
            //     {
            //         // Not enough room
            //         return false;
            //     }
            //     weaponItems.Add(item);
            // }

            // // If there is enough room, add the item and invoke the delegate so that the other methods can be notified
            // items.Add(item);

            // onItemChangedCallback?.Invoke();

        }
        return true;
    }

    bool itemAddCheck(Item item, List<Item> items)
    {
        if (items.Count >= space)
        {
            // Not enough room
            return false;
        }
        items.Add(item);
        return true;
    }


    public void Remove(Item item)
    {
        // Remove the item and also invoke the delegate so that the other methods can be notified
        if (item.type == Type.ARMOR)
        {
            armorItems.Remove(item);
        }
        if (item.type == Type.WEAPON)
        {
            weaponItems.Remove(item);
        }
        items.Remove(item);

        onItemChangedCallback?.Invoke();
    }


    void ToggleInventory()
    {
        // TODO: This needs to be changed for the new input system

        // Toggles the inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
    }
}
