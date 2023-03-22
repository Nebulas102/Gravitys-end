using ScriptableObjects;
using UnityEngine;

namespace Core.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        public Item item;
        
        private void OnMouseDown()
        {
            // ONLY pick up ARMOR AND WEAPON ITEMS
            if (item.type is not (Type.ARMOR or Type.WEAPON))
                return;

            // Boolean to check if i was picked up (maybe the inventory was full or not)
            bool wasPickedUp = Inventory.instance.Add(item);

            // If it was picked up, destroy that object from the scene
            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
}
