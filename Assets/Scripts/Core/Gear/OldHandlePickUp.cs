using Core.UI.Inventory;
using ScriptableObjects;
using UnityEngine;

public class OldHandlePickUp : MonoBehaviour
{
        private Item item;
        private bool _canPickUp;
        private GameInput _gameInput;
        private bool _isPickingUp;

        private void Awake()
        {
            item = gameObject.GetComponent<BaseItem>().item;
            _gameInput = FindObjectOfType<GameInput>();
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

            if (gameObject.GetComponent<BaseItem>().isInInventory)
                return;    

            // Boolean to check if i was picked up (maybe the inventory was full or not)
            var wasPickedUp = Inventory.Instance.Add(gameObject);

            // If it was picked up, set the object unactive
            if (wasPickedUp)
                gameObject.SetActive(false);
        }
}
