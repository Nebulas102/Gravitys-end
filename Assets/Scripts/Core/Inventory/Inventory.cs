using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.Inventory
{
    public class Inventory : MonoBehaviour
    {
        // Delegate 
        public delegate void OnItemChanged();
        /* 
            Add this method to the player movement controller Update() function, so when the inventory is opened the other movement actions wont be executed
            
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
        
        */

        // Singleton for inventory
        public static Inventory instance;

        // Space for each type of item
        public int space = 3;
        public GameObject inventoryUI;
        public readonly IDictionary<Type, List<Item>> items = new Dictionary<Type, List<Item>>();

        private UIMenus _UIMenus;
        private bool inventoryToggleInput;
        public OnItemChanged onItemChangedCallback;


        private void Awake()
        {
            _UIMenus = new UIMenus();

            if (instance != null)
                return;

            instance = this;
            items.Add(Type.ARMOR, new List<Item>());
            items.Add(Type.WEAPON, new List<Item>());
        }

        private void Update()
        {
            ToggleInventory();
        }

        private void OnEnable()
        {
            _UIMenus.Enable();
            _UIMenus.Menus.ToggleInventory.performed += ctx => inventoryToggleInput = true;
        }

        private void OnDisable()
        {
            _UIMenus.Disable();
        }

        public bool Add(Item item)
        {
            if (item.isDefaultItem)
                return true;

            // Check whether the dictionary key is already set, if not, set with new list
            if (!items.TryGetValue(item.type, out var list))
            {
                list = new List<Item>();
                items.Add(item.type, list);
            }

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
            var list = items.First(x => x.Key == item.type).Value;
            list.Remove(item);

            items[item.type] = list;

            onItemChangedCallback?.Invoke();
        }


        private void ToggleInventory()
        {
            // Toggles the inventory
            if (inventoryToggleInput) inventoryUI.SetActive(!inventoryUI.activeSelf);

            inventoryToggleInput = false;
        }

        public void CloseInventory()
        {
            inventoryUI.SetActive(false);
        }
    }
}
