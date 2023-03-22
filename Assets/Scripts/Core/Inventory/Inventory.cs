using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.Inventory
{
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
        public readonly IDictionary<Type, List<Item>> items = new Dictionary<Type, List<Item>>();

        private void Awake()
        {
            if (instance != null)
                return;
            
            instance = this;
        }
        
        private void Update()
        {
            ToggleInventory();
        }

        public bool Add(Item item)
        {
            if (item.isDefaultItem)
                return true;
            
            // Check whether the dictionary key is already set, if not, set with new list
            if (!items.ContainsKey(item.type))
                items.Add(item.type, new List<Item>());

            // Get the inventory list
            List<Item> list = items.First(x => x.Key == item.type).Value;
            if (list.Count >= space)
                return false;
            
            // Add item to list and update the dictionary with the updated list
            list.Add(item);
            items[item.type] = list;

            onItemChangedCallback?.Invoke();
            return true;
        }

        public void Remove(Item item)
        {
            // Remove the item and also invoke the delegate so that the other methods can be notified
            List<Item> list = items.First(x => x.Key == item.type).Value;
            list.Remove(item);

            items[item.type] = list;

            onItemChangedCallback?.Invoke();
        }


        private void ToggleInventory()
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
}
