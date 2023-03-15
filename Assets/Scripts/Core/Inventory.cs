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


    public int space = 6;
    public GameObject inventoryUI;
    public List<Item> items = new List<Item>();


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
        // TODO: This needs to be changed for the new input system

        // Toggles the inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public bool Add(Item item)
    {
        // Don't add the item if it is default, maybe it is an item which isn't supposed to be in the inventory
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                // Not enough room
                return false;
            }

            // If there is enough room, add the item and invoke the delegate so that the other methods can be notified
            items.Add(item);

            onItemChangedCallback?.Invoke();

        }
        return true;
    }


    public void Remove(Item item)
    {
        // Remove the item and also invoke the delegate so that the other methods can be notified
        items.Remove(item);

        onItemChangedCallback?.Invoke();
    }

}
