using System.Collections.Generic;
using Core.Audio;
using UnityEngine;

namespace Core.Chest
{
    public class Chest : MonoBehaviour
    {
        [SerializeField]
        private GameObject openedChestGameObject;

        [SerializeField]
        [Tooltip("The radius to detect chests")]
        private float detectionRadius = 2f;

        [SerializeField]
        [Tooltip("The distance from the chest to spawn the item")]
        private float itemSpawnDistanceFromChest = 1f;

        private List<LootItem> _lootObjects;
        private InputManager _inputManager;
        private GameObject _player;
        private bool _chestOpened;
        private bool _chestOpeningInput;
        private bool _canOpen;

        public delegate void ChestOpenHandler(bool canOpen);
        public static event ChestOpenHandler OnChestOpen;

        private void Awake()
        {
            _inputManager = new InputManager();
            _player = PlayerManager.Instance.player;
            _lootObjects = new();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_canOpen)
                OpenChest();
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.Player.OpenChest.performed += _ => OnOpenChestInput();
        }

        private void OnDisable()
        {
            _inputManager.Disable();
        }

        public void SetLootObjects(List<LootItem> lootObjects)
        {
            _lootObjects = lootObjects;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ToggleCanOpen(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && _canOpen)
                ToggleCanOpen(false);
        }

        private void OpenChest()
        {
            // When the chest isn't opened yet, the player can open the chest.
            if (!_chestOpeningInput)
                return;

            // Code for changing the chest state to open
            foreach (Transform child in transform) Destroy(child.gameObject);

            // Instantiate the serialized GameObject as a child of the parent GameObject
            var newChild = Instantiate(openedChestGameObject, transform);
            newChild.transform.localPosition = Vector3.zero;

            SoundEffectsManager.instance.PlaySoundEffect(SoundEffect.CHEST_OPENING);

            SpawnLoot();

            // Force the chest to be unusable after opening
            _chestOpeningInput = false;
            _chestOpened = true;
            ToggleCanOpen(false);
        }

        private void SpawnLoot()
        {
            List<int> randomizer = new();
            for (int i = 0; i < _lootObjects.Count; i++)
                for (int j = 0; j < _lootObjects[i].spawnChanceWeight; j++)
                    randomizer.Add(i);

            int randomInt = Random.Range(0, randomizer.Count);
            int randomizerResult = randomizer[randomInt];

            Vector3 spawnPosition = GetRandomSpawnPosition();

            Instantiate(_lootObjects[randomizerResult].item, spawnPosition, Quaternion.identity);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return transform.position + transform.forward * itemSpawnDistanceFromChest;
        }

        private void OnOpenChestInput()
        {
            _chestOpeningInput = !_chestOpened && _canOpen;
        }

        private void ToggleCanOpen(bool canOpen)
        {
            OnChestOpen?.Invoke(canOpen);
            _canOpen = canOpen;
        }
    }
}