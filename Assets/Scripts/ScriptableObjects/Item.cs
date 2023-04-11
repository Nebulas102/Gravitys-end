using System;
using Core.UI.Inventory;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        public Sprite icon;
        public bool isDefaultItem;

        [SerializeField]
        public Type type;

        public void Use(InventorySlot itemSlot)
        {
            switch (type)
            {
                case Type.ARMOR when itemSlot.isCurrentSlot:
                {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the current slot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.Instance.CheckForAvailableSlots(Type.ARMOR) == null) return;

                    InventoryUI.Instance.CheckForAvailableSlots(Type.ARMOR)
                        .AddItem(InventoryUI.StaticCurrentArmorSlot.GetItem());
                    Inventory.Instance.Add(InventoryUI.StaticCurrentArmorSlot.GetItem());

                    InventoryUI.StaticCurrentArmorSlot.ClearSlot();
                    break;
                }
                // Remove from the current slot; 
                case Type.ARMOR:
                {
                    itemSlot.ClearSlot();
                    var oldItem = InventoryUI.StaticCurrentArmorSlot.GetItem();

                    if (oldItem != null)
                    {
                        itemSlot.AddItem(oldItem);
                        Inventory.Instance.Add(oldItem);
                    }

                    InventoryUI.StaticCurrentArmorSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.StaticCurrentArmorSlot.AddItem(this);
                    Inventory.Instance.Remove(this);
                    break;
                }
                case Type.WEAPON when itemSlot.isCurrentSlot:
                {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the current slot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.Instance.CheckForAvailableSlots(Type.WEAPON) == null) return;

                    InventoryUI.Instance.CheckForAvailableSlots(Type.WEAPON)
                        .AddItem(InventoryUI.StaticCurrentWeaponSlot.GetItem());
                    Inventory.Instance.Add(InventoryUI.StaticCurrentWeaponSlot.GetItem());
                    InventoryUI.StaticCurrentWeaponSlot.ClearSlot();

                    return;
                }
                // Remove from the current slot; 
                case Type.WEAPON:
                {
                    itemSlot.ClearSlot();
                    var oldItem = InventoryUI.StaticCurrentWeaponSlot.GetItem();

                    if (oldItem != null)
                    {
                        itemSlot.AddItem(oldItem);
                        Inventory.Instance.Add(oldItem);
                    }

                    InventoryUI.StaticCurrentWeaponSlot.ClearSlot();

                    // Add this item to the currentSlot
                    InventoryUI.StaticCurrentWeaponSlot.AddItem(this);
                    Inventory.Instance.Remove(this);
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
