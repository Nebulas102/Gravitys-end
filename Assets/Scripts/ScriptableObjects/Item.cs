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
                    break;
                }
                // Remove from the current slot; 
                case Type.ARMOR:
                {
                    itemSlot.ClearSlot();
                    var oldItem = InventoryUI.staticCurrentArmorSlot.GetItem();

                    if (oldItem != null)
                    {
                        itemSlot.AddItem(oldItem);
                        Inventory.instance.Add(oldItem);
                    }

                    InventoryUI.staticCurrentArmorSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.staticCurrentArmorSlot.AddItem(this);
                    Inventory.instance.Remove(this);
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
                    var oldItem = InventoryUI.staticCurrentWeaponSlot.GetItem();

                    if (oldItem != null)
                    {
                        itemSlot.AddItem(oldItem);
                        Inventory.instance.Add(oldItem);
                    }

                    InventoryUI.staticCurrentWeaponSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.staticCurrentWeaponSlot.AddItem(this);
                    Inventory.instance.Remove(this);
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
