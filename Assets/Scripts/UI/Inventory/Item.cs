using UnityEngine;

namespace UI.Inventory
{
    public class Item : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        [Tooltip("The icon to display in the inventory")]
        public Sprite icon;

        [SerializeField]
        [Tooltip("The type of item this is")]
        public ItemType type;

        [Header("Gameplay")]
        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("The amount of damage or armor increase this item gives in percentage")]
        private float modifier;

        [SerializeField]
        [Tooltip("The prefab to spawn when this item is dropped")]
        private GameObject prefab;

        private void Awake()
        {
            var inventory = FindObjectOfType<InventoryManager>();
            inventory.PickupItem(this);
        }

        public void Spawn(Vector3 position)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }

        public float GetModifier()
        {
            return 1 + (modifier / 100);
        }
    }


    public enum ItemType
    {
        [InspectorName("Weapon")]
        WEAPON,

        [InspectorName("Armor")]
        ARMOR,
    }
}