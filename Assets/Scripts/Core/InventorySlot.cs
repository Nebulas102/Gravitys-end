using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] public bool isCurrentSlot = false;
    [SerializeField] public Image icon;
    [SerializeField] public Button removeButton;
    [SerializeField] public Type type;

    Item item;


    public void AddItem(Item newItem)
    {
        // item and its contents are to be initialized
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        removeButton.interactable = true;

    }

    public void ClearSlot()
    {
        // item and its contents are set to null
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        removeButton.interactable = false;

    }


    public Item GetItem()
    {
        if (item != null)
        {
            return item;
        }
        return null;
    }


    public void OnRemoveButton()
    {
        // Something for later: Drop the item on the ground

        // If the current slot calls remove then clear that slot and get out of the function 
        // without removing the item from the array in inventory
        if (this.isCurrentSlot)
        {
            this.ClearSlot();
            return;
        }

        Inventory.instance.Remove(item);
    }



    public void UseItem()
    {

        // When the slot gets clicked and the slot isn't empty
        // Then you can use that particular item
        if (item != null)
        {

            item.Use(this);
        }
    }


}
