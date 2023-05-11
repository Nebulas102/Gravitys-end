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

        public bool inventoryOpened;

        public readonly IDictionary<Type, List<GameObject>> Items = new Dictionary<Type, List<GameObject>>();
        private bool _inventoryToggleInput;
        private bool closeMenuInput;

        private GameObject _player;

        private UIMenus _uiMenus;
        public OnItemChanged OnItemChangedCallback;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            _uiMenus = new UIMenus();

            Items.Add(Type.ARMOR, new List<GameObject>());
            Items.Add(Type.WEAPON, new List<GameObject>());
        }

        private void Start()
        {
            _player = GameObject.Find("Player");
        }

        private void Update()
        {
            ToggleInventory();
        }

        private void OnEnable()
        {
            _uiMenus.Enable();
            _uiMenus.Menus.ToggleInventory.performed += _ => _inventoryToggleInput = true;
            _uiMenus.Menus.CloseMenu.performed += ctx => closeMenuInput = true;
        }

        private void OnDisable()
        {
            _uiMenus.Disable();
        }

        public bool Add(GameObject itemObject)
        {
            // Debug.Log(itemObject.name);
            Item item = itemObject.GetComponent<BaseItem>().item;

            itemObject.GetComponent<BaseItem>().isInInventory = true;

            if (item.isDefaultItem)
                return true;

            // Check whether the dictionary key is already set, if not, set with new list
            if (!Items.TryGetValue(item.type, out var list))
            {
                list = new List<GameObject>();
                Items.Add(item.type, list);
            }

            if (list.Count >= space)
                return false;

            // Add item to list and update the dictionary with the updated list
            list.Add(itemObject);
            Items[item.type] = list;

            OnItemChangedCallback?.Invoke();
            return true;
        }

        public void RemoveDrop(GameObject itemObject)
        {
            RemoveSlot(itemObject);

            // Spawn the item back on the ground
            itemObject.GetComponent<BaseItem>().item.Spawn(_player.transform.position);

            itemObject.GetComponent<BaseItem>().isInInventory = false;

            OnItemChangedCallback?.Invoke();
        }

        public void RemoveSlot(GameObject itemObject)
        {
            Item item = itemObject.GetComponent<BaseItem>().item;
            // Remove the item and also invoke the delegate so that the other methods can be notified
            var list = Items.First(x => x.Key == item.type).Value;
            list.Remove(itemObject);

            Items[item.type] = list;

            OnItemChangedCallback?.Invoke();
        }

        private void ToggleInventory()
        {
            if (PauseMenu.instance.isPaused) return;
            if (!inventoryOpened) inventoryUI.SetActive(false);
            // Toggles the inventory
            if (_inventoryToggleInput)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
                inventoryOpened = inventoryUI.activeSelf;
            }

            if (closeMenuInput)
            {
                if (inventoryOpened)
                {
                    CloseInventory();
                }
            }

            _inventoryToggleInput = false;
            closeMenuInput = false;
        }

        public void CloseInventory()
        {
            inventoryUI.SetActive(false);
            inventoryOpened = inventoryUI.activeSelf;
        }
    }
}
