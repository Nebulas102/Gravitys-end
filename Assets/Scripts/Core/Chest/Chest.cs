using System.Collections.Generic;
using UnityEngine;

namespace Core.Chest
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private GameObject openedChestGameObject;

        [SerializeField] private float detectionRadius = 2f; // The radius to detect chests

        public List<GameObject> lootObjects;

        private InputManager _inputManager;

        private GameObject _player;

        private bool chestOpened;
        private bool chestOpeningInput;

        private void Awake()
        {
            _inputManager = new InputManager();
            _player = PlayerManager.Instance.player;
            lootObjects = new();
        }

        // Update is called once per frame
        private void Update()
        {
            OpenChest();
            chestOpeningInput = false;
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.Player.OpenChest.performed += ctx => chestOpeningInput = true;
        }

        private void OnDisable()
        {
            _inputManager.Disable();
        }

        public void SetLootObjects(List<GameObject> _lootObjects)
        {
            lootObjects = _lootObjects;
        }

        private void OpenChest()
        {
            if (!IsPlayerNearby()) return;

            // When the chest isn't opened yet, the player can open the chest.
            if (chestOpeningInput && !chestOpened)
            {
                chestOpened = true;

                // Code for changing the chest state to open
                foreach (Transform child in transform) Destroy(child.gameObject);

                // Instantiate the serialized GameObject as a child of the parent GameObject
                var newChild = Instantiate(openedChestGameObject, transform);
                newChild.transform.localPosition = Vector3.zero;

                SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.ChestOpening);

                SpawnLoot();
            }
        }

        public bool IsPlayerNearby()
        {
            if (_player is null) return false;

            var distance = Vector3.Distance(transform.position, _player.transform.position);
            return distance <= detectionRadius;
        }

        public void SpawnLoot()
        {
            // Randomly select a subset of items to spawn
            var selectedLootObjects = new List<GameObject>();
            foreach (var lootObject in lootObjects)
                if (Random.value <= 0.5f) // Adjust the probability as needed (e.g., 0.5f for 50% chance)
                    selectedLootObjects.Add(lootObject);

            foreach (var lootObject in selectedLootObjects)
            {
                var spawnPosition = GetRandomSpawnPosition();
                Instantiate(lootObject, spawnPosition, Quaternion.identity);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            var offset = 1.0f; // Distance from the chest where items should spawn
            Vector3 randomOffset = Random.insideUnitCircle.normalized * offset;
            var spawnPosition = transform.position + randomOffset;

            // Apply margin to the spawn position
            var direction = (spawnPosition - transform.position).normalized;
            spawnPosition += direction * offset;

            // Get the size of the chest collider
            var chestCollider = GetComponent<Collider>();
            var chestSize = chestCollider.bounds.size;

            // Calculate the minimum distance from the chest where loot should spawn
            var minDistance = Mathf.Max(chestSize.x, chestSize.z) / 2.0f;

            // Set y-level to 0
            spawnPosition.y = 0.0f;

            // Check if the spawn position is inside or too close to the chest bounds
            while (IsPositionInsideChestBounds(spawnPosition, chestCollider) ||
                   Vector3.Distance(spawnPosition, transform.position) < minDistance)
            {
                randomOffset = Random.insideUnitCircle.normalized * offset;
                spawnPosition = transform.position + randomOffset;
                spawnPosition += direction * offset;
                spawnPosition.y = 0.0f;
            }

            return spawnPosition;
        }

        private bool IsPositionInsideChestBounds(Vector3 position, Collider chestCollider)
        {
            var closestPoint = chestCollider.ClosestPoint(position);
            return closestPoint == position;
        }
    }
}