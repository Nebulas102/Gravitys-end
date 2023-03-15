using Assets.Scripts.ScriptableObjects;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public Item item;


    private void OnMouseDown()
    {
        // Boolean to check if i was picked up (maybe the inventory was full or not)
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {
            // If it was picked up, destroy that object from the scene
            Destroy(gameObject);
        }
    }



}
