using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Core.UI.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public static InventorySlot StaticCurrentArmorSlot;
        public static InventorySlot StaticCurrentWeaponSlot;

        // Singleton for inventoryUI
        public static InventoryUI Instance;

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

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Get the singleton instance of the inventory and subscribe to the delegate
            Inventory.Instance.OnItemChangedCallback += UpdateGUI;

            // Set the current slots to the static one's in order to obtain the right ones in other scripts
            StaticCurrentArmorSlot = currentArmorSlot;
            StaticCurrentWeaponSlot = currentWeaponSlot;

            UpdateGUI();
        }

        private void UpdateGUI()
        {
            UpdateSlots(armorSlots, Type.ARMOR);
            UpdateSlots(weaponSlots, Type.WEAPON);
        }

        private static void UpdateSlots(List<InventorySlot> inventorySlots, Type itemType)
        {
            var items = Inventory.Instance.Items[itemType];

            foreach (var slot in inventorySlots)
            {
                var index = inventorySlots.IndexOf(slot);

                if (index < items.Count)
                {
                    slot.AddItem(items[index]);
                }
                else
                    slot.ClearSlot();
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
                Type.ARMOR => GetFirstAvailableSlot(armorSlots,
                    Inventory.Instance.Items.First(x => x.Key == Type.ARMOR).Value),
                Type.WEAPON => GetFirstAvailableSlot(weaponSlots,
                    Inventory.Instance.Items.First(x => x.Key == Type.WEAPON).Value),
                _ => null
            };
        }
    }
}
