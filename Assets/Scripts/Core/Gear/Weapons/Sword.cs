using Core.UI.Inventory;
using ScriptableObjects;
using UnityEngine;

namespace Core.Gear.Weapons
{
    public class Sword : MonoBehaviour
    {
        public Item item;
        private GameInput _gameInput;
        private bool _isPickingUp;

        private void Awake()
        {
            _gameInput = FindObjectOfType<GameInput>();
        }

        private void Update()
        {
            _isPickingUp = _gameInput.GetPickUp();
        }

        private void OnTriggerStay(Collider hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Entity")) HandlePickup();
        }

        private void HandlePickup()
        {
            if (!_isPickingUp) return;

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
