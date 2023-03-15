using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    [SerializeField] public bool isCurrentSlot = false;
    [SerializeField] public Image icon;
    [SerializeField] public Button removeButton;

    Color color;

    Item item;

    void Start()
    {
        color = removeButton.GetComponent<Image>().color;
    }

    public void AddItem(Item newItem)
    {
        // item and its contents are be initialized
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        // Code beneath is for when the slot is equipped with an item
        // so the remove button will be interactable and the color of it will go to 1f to see the button
        removeButton.interactable = true;
        color.a = 1f;
        removeButton.image.color = color;

    }

    public void ClearSlot()
    {
        // item and its contents are set to null
        item = null;
        icon.sprite = null;
        icon.enabled = false;

        // Code beneath is for when the slot is cleared
        // so the remove button will not be interactable and the opacity of it will go to transparent
        removeButton.interactable = false;
        color.a = 0f;
        removeButton.image.color = color;

    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }


    public void UseItem () {
        // When the slot gets clicked and the slot isn't empty
        // Then you can use that particular item
        if (item != null) {
            item.Use();
        }
    }


}
