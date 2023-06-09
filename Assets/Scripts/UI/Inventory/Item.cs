using TMPro;
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
        public GameObject prefab;

        [SerializeField]
        [Range(0f, 2f)]
        [Tooltip("The radius around the item that the player must be in to be able to pick it up")]
        private float radius = 2f;

        [SerializeField]
        [Tooltip("The mesh renderer of the item")]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private float verticalMotionSpeed = 4f; // Adjust the speed as desired

        [SerializeField]
        private float verticalMotionAmplitude = 1f; // Adjust the vertical motion amplitude as desired


        [HideInInspector]
        public bool IsInInventory;

        public delegate void ItemPickupEventHandler(bool canPickup);
        public static event ItemPickupEventHandler OnItemPickup;

        private GameObject _player;
        private GameInput _gameInput;
        private bool _inventoryOpened;
        private Vector3 originalPosition;
        private bool isPlayerNearby;
        private bool isShowingPrompt;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _gameInput = FindObjectOfType<GameInput>();
            InventoryOverlayBehaviour.OnInventoryToggle += ToggleInventory;

            originalPosition = transform.position;
        }

        public void ToggleInventory(bool inventoryOpened)
        {
            _inventoryOpened = inventoryOpened;
        }

        private void Update()
        {
            Pickup();
        }

        private void FixedUpdate()
        {
            ShowPrompt();
        }

        public void Spawn(Vector3 position)
        {
            IsInInventory = false;
            RenderItem(true);
            gameObject.transform.position = position;
            isPlayerNearby = true;
            OnItemPickup?.Invoke(false);
        }

        public void Spawn()
        {
            Spawn(_player.transform.position);
        }

        public float GetModifier()
        {
            return 1 + (modifier / 100);
        }

        public bool IsPlayerNearby()
        {
            if (meshRenderer.enabled == false || _player is null || IsInInventory) return false;

            var distance = Vector3.Distance(transform.position, _player.transform.position);
            return distance <= radius;
        }

        private void Pickup()
        {
            if (!_gameInput.GetPickUp() || !IsPlayerNearby() || IsInInventory || _inventoryOpened) return;

            InventoryManager.instance.PickupItem(this);
            meshRenderer.enabled = false;
            IsInInventory = true;
            isPlayerNearby = false;
            OnItemPickup?.Invoke(false);
            
            if (this.tag == "Melee" || this.tag == "Ranged") { 
                SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.GunPickup);
            }
        }

        public void RenderItem(bool render)
        {
            meshRenderer.enabled = render;
        }

        public void ShowPrompt()
        {
            // Check if a player is nearby the item
            if (IsPlayerNearby())
            {
                if (!isPlayerNearby)
                {
                    isPlayerNearby = true;

                    // Start showing prompt, store current position as original position
                    originalPosition = transform.position;
                    OnItemPickup?.Invoke(true);
                    isShowingPrompt = true;
                }
                // Calculate the vertical offset based on a smooth oscillation using Mathf.Sin
                float verticalOffset = Mathf.Sin(Time.time * verticalMotionSpeed) * verticalMotionAmplitude;

                // Set the target position with the vertical offset applied
                Vector3 targetPosition = originalPosition + Vector3.up * verticalOffset;

                // Clamp the target position to stay at or above y = 0
                targetPosition.y = Mathf.Max(targetPosition.y, 0.3f);

                // Smoothly move the item towards the target position using Lerp
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * verticalMotionSpeed);
            }
            else
            {
                if (isPlayerNearby)
                {
                    isPlayerNearby = false;

                    // Stop showing prompt, reset item position
                    // transform.position = originalPosition;
                    OnItemPickup?.Invoke(false);
                    isShowingPrompt = false;
                }
            }
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