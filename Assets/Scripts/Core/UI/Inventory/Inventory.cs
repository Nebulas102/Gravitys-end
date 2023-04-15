using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        // Delegate 
        public delegate void OnItemChanged();

        // Singleton for inventory
        public static Inventory Instance;

        // Space for each type of item
        [SerializeField]
        public int space = 3;

        [SerializeField]
        public GameObject inventoryUI;

        public bool inventoryOpened = false;

        public readonly IDictionary<Type, List<Item>> Items = new Dictionary<Type, List<Item>>();
        private bool _inventoryToggleInput;

        private UIMenus _uiMenus;
        public OnItemChanged OnItemChangedCallback;


        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _uiMenus = new UIMenus();

            Items.Add(Type.ARMOR, new List<Item>());
            Items.Add(Type.WEAPON, new List<Item>());
        }

        private void Update()
        {
            ToggleInventory();
        }

        private void OnEnable()
        {
            _uiMenus.Enable();
            _uiMenus.Menus.ToggleInventory.performed += _ => _inventoryToggleInput = true;
        }

        private void OnDisable()
        {
            _uiMenus.Disable();
        }

        public bool Add(Item item)
        {
            if (item.isDefaultItem)
                return true;

            // Check whether the dictionary key is already set, if not, set with new list
            if (!Items.TryGetValue(item.type, out var list))
            {
                list = new List<Item>();
                Items.Add(item.type, list);
            }

            if (list.Count >= space)
                return false;

            // Add item to list and update the dictionary with the updated list
            list.Add(item);
            Items[item.type] = list;

            OnItemChangedCallback?.Invoke();
            return true;
        }

        public void Remove(Item item)
        {
            // Remove the item and also invoke the delegate so that the other methods can be notified
            var list = Items.First(x => x.Key == item.type).Value;
            list.Remove(item);

            Items[item.type] = list;

            OnItemChangedCallback?.Invoke();
        }

        private void ToggleInventory()
        {
            if (PauseMenu.instance.isPaused)
            {
                return;
            }
            if (!inventoryOpened)
            {
                inventoryUI.SetActive(false);
            }
            // Toggles the inventory
            if (_inventoryToggleInput)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
                inventoryOpened = inventoryUI.activeSelf;
            }

            _inventoryToggleInput = false;
        }

        public void CloseInventory()
        {
            inventoryUI.SetActive(false);
            inventoryOpened = inventoryUI.activeSelf;
        }
    }
}
