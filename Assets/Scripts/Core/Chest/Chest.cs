using System.Collections.Generic;
using Core.Audio;
using UnityEngine;

namespace Core.Chest
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private GameObject openedChestGameObject;

        [SerializeField] private float detectionRadius = 2f; // The radius to detect chests
        [SerializeField] private float itemSpawnDistanceFromChest = 1f;

        public List<LootItem> lootObjects;

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

        public void SetLootObjects(List<LootItem> _lootObjects)
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

                newChild.GetComponentInChildren<ParticleSystem>().Play();
                SoundEffectsManager.instance.PlaySoundEffect(SoundEffect.CHEST_OPENING);

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
            List<int> randomizer = new();
            for(int i = 0; i < lootObjects.Count; i++)
                for(int j = 0; j < lootObjects[i].spawnChanceWeight; j++)
                    randomizer.Add(i);

            int randomInt = Random.Range(0, randomizer.Count);
            int randomizerResult = randomizer[randomInt];

            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(lootObjects[randomizerResult].item, spawnPosition, Quaternion.identity);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 spawnPosition = transform.position + transform.forward * itemSpawnDistanceFromChest;

            return spawnPosition;
        }
    }
}