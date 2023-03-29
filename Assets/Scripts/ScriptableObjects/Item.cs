using System;
using Core.Inventory;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite icon;
        public bool isDefaultItem;

        [SerializeField]
        public Type type;

        public virtual void Use(InventorySlot itemSlot)
        {
            switch (type)
            {
                case Type.ARMOR when itemSlot.isCurrentSlot:
                {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the current slot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.instance.CheckForAvailableSlots(Type.ARMOR) == null) return;

                    InventoryUI.instance.CheckForAvailableSlots(Type.ARMOR)
                        .AddItem(InventoryUI.staticCurrentArmorSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentArmorSlot.GetItem());
                    InventoryUI.staticCurrentArmorSlot.ClearSlot();
                    return;
                }
                // Remove from the current slot; 
                case Type.ARMOR:
                {
                    itemSlot.ClearSlot();
                    Inventory.instance.Remove(this);

                    // Check if the current slot has an item, if so, put the item of the current slot in this slot
                    // and put this item in the current slot (Swapping the items.)
                    if (InventoryUI.staticCurrentArmorSlot.GetItem() == null) return;

                    itemSlot.AddItem(InventoryUI.staticCurrentArmorSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentArmorSlot.GetItem());
                    InventoryUI.staticCurrentArmorSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.staticCurrentArmorSlot.AddItem(this);
                    break;
                }
                case Type.WEAPON when itemSlot.isCurrentSlot:
                {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the current slot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.instance.CheckForAvailableSlots(Type.WEAPON) == null) return;

                    InventoryUI.instance.CheckForAvailableSlots(Type.WEAPON)
                        .AddItem(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    InventoryUI.staticCurrentWeaponSlot.ClearSlot();

                    return;
                }
                // Remove from the current slot; 
                case Type.WEAPON:
                {
                    itemSlot.ClearSlot();
                    Inventory.instance.Remove(this);

                    // Check if the current slot has an item, if so, put the item of the current slot in this slot
                    // and put this item in the current slot (Swapping the items.)
                    if (InventoryUI.staticCurrentWeaponSlot.GetItem() == null) return;

                    itemSlot.AddItem(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    InventoryUI.staticCurrentWeaponSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.staticCurrentWeaponSlot.AddItem(this);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum Type
    {
        WEAPON,
        ARMOR
    }
}
