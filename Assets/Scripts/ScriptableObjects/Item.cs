using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite icon = null;
        public bool isDefaultItem = false;
        [SerializeField] public Type type;



        public virtual void Use(InventorySlot itemSlot)
        {
            if (this.type == Type.ARMOR) {

                if (itemSlot.isCurrentSlot) {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the currentslot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.instance.CheckForAvailableSlots(Type.ARMOR) != null) {
                        InventoryUI.instance.CheckForAvailableSlots(Type.ARMOR).AddItem(InventoryUI.staticCurrentArmorSlot.GetItem());
                        Inventory.instance.Add(InventoryUI.staticCurrentArmorSlot.GetItem());
                        InventoryUI.staticCurrentArmorSlot.ClearSlot();
                    }
                    return;
                }

                // Remove from the current slot; 
                itemSlot.ClearSlot();
                Inventory.instance.Remove(this);

                // Check if the current slot has an item, if so, put the item of the current slot in this slot
                // and put this item in the current slot (Swapping the items.)
                if (InventoryUI.staticCurrentArmorSlot.GetItem() != null) {
                    itemSlot.AddItem(InventoryUI.staticCurrentArmorSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentArmorSlot.GetItem());
                    InventoryUI.staticCurrentArmorSlot.ClearSlot();
                }

                // Add this item to the currentSlot
                InventoryUI.staticCurrentArmorSlot.AddItem(this);
            }

            if (this.type == Type.WEAPON) {
                
                if (itemSlot.isCurrentSlot) {
                    // DISCONTINUED cant figure this one out so the best solution is to use the remove button in the inventory

                    // Check if other (non current slots) are available to put this item in
                    // If so remove everything from the currentslot and put it in the available one
                    // Else dont do anything
                    if (InventoryUI.instance.CheckForAvailableSlots(Type.WEAPON) != null) {
                        InventoryUI.instance.CheckForAvailableSlots(Type.WEAPON).AddItem(InventoryUI.staticCurrentWeaponSlot.GetItem());
                        Inventory.instance.Add(InventoryUI.staticCurrentWeaponSlot.GetItem());
                        InventoryUI.staticCurrentWeaponSlot.ClearSlot();
                    }
                    return;
                }

                // Remove from the current slot; 
                itemSlot.ClearSlot();
                Inventory.instance.Remove(this);

                // Check if the current slot has an item, if so, put the item of the current slot in this slot
                // and put this item in the current slot (Swapping the items.)
                if (InventoryUI.staticCurrentWeaponSlot.GetItem() != null) {
                    itemSlot.AddItem(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    Inventory.instance.Add(InventoryUI.staticCurrentWeaponSlot.GetItem());
                    InventoryUI.staticCurrentWeaponSlot.ClearSlot();
                }

                // Add this item to the currentSlot
                InventoryUI.staticCurrentWeaponSlot.AddItem(this);
            }

        }


    }

    public enum Type
    {
        WEAPON,
        ARMOR
    }

}


