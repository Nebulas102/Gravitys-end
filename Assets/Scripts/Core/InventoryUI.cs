using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public Transform itemsParent;

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        // Get the singleton instance of the inventory and subscribe to the delegate
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateGUI;

        // Get all the inventory slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }


    void UpdateGUI()
    {
        // Loop through all slots when the itemchanged delegate is invoked. So the inventory GUI gets updated
        for (int i = 0; i < slots.Length; i++)
        {
            // If an item is to be added
            if (i < inventory.items.Count)
            {
                // Add it
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                // If not then clear the slot
                slots[i].ClearSlot();
            }

        }
    }
}
