using Core.UI.Inventory;
using ScriptableObjects;
using UnityEngine;

namespace Core.Gear.Weapons
{
    public class Sword : MonoBehaviour
    {
        [SerializeField]
        private float yCoordinate;

        public Item item;
        private bool _canPickUp;
        private GameInput _gameInput;
        private bool _isPickingUp;

        private void Awake()
        {
            _gameInput = FindObjectOfType<GameInput>();

            // Reset y position for better collision detection
            var position = transform.position;
            transform.SetPositionAndRotation(new Vector3(position.x, yCoordinate, position.z), Quaternion.identity);
        }

        private void Update()
        {
            _isPickingUp = _gameInput.GetPickUp();
            HandlePickup();
        }

        private void OnTriggerExit(Collider hit)
        {
            _canPickUp = false;
        }

        private void OnTriggerStay(Collider hit)
        {
            _canPickUp = hit.gameObject.layer == LayerMask.NameToLayer("Entity");
        }

        private void HandlePickup()
        {
            if (!_isPickingUp || !_canPickUp) return;

            if (item.type is not (Type.ARMOR or Type.WEAPON))
                return;

            // Boolean to check if i was picked up (maybe the inventory was full or not)
            var wasPickedUp = Inventory.Instance.Add(item);

            // If it was picked up, destroy that object from the scene
            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
}
