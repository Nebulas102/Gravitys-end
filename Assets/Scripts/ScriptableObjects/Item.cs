using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite icon = null;
        
        public bool isDefaultItem = false;

        public virtual void Use()
        {
            // Put this item in the currentSlot inventory slot
            Debug.Log("Using " + Name);
        }




    }
}


