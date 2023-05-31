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

        [SerializeField]
        [Range(0f, 2f)]
        [Tooltip("The radius around the item that the player must be in to be able to pick it up")]
        private float radius = 2f;

        [SerializeField]
        [Tooltip("The mesh renderer of the item")]
        private MeshRenderer meshRenderer;

        [HideInInspector]
        public bool IsInInventory;

        private GameObject _player;
        private GameInput _gameInput;
        private bool _inventoryOpened;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _gameInput = FindObjectOfType<GameInput>();
            InventoryOverlayBehaviour.OnInventoryToggle += ToggleInventory;
        }

        public void ToggleInventory(bool inventoryOpened)
        {
            _inventoryOpened = inventoryOpened;
        }

        private void Update()
        {
            Pickup();
        }

        public void Spawn()
        {
            IsInInventory = false;
            meshRenderer.enabled = true;
            gameObject.transform.position = _player.transform.position;
        }

        public float GetModifier()
        {
            return 1 + (modifier / 100);
        }

        public bool IsPlayerNearby()
        {
            if (_player is null) return false;

            var distance = Vector3.Distance(transform.position, _player.transform.position);
            return distance <= radius;
        }

        private void Pickup()
        {
            if (!_gameInput.GetPickUp() || !IsPlayerNearby() || IsInInventory || _inventoryOpened) return;

            InventoryManager.instance.PickupItem(this);
            meshRenderer.enabled = false;
            IsInInventory = true;
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