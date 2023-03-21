using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public Item item;


    private void OnMouseDown()
    {
        // ONLY pick up ARMOR AND WEAPON ITEMS
        if (!(item.type == Type.ARMOR || item.type == Type.WEAPON)) {
            return;
        }

        // Boolean to check if i was picked up (maybe the inventory was full or not)
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {
            // If it was picked up, destroy that object from the scene
            Destroy(gameObject);
        }
    }



}
