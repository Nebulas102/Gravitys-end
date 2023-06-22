using Controllers.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        public GameObject iconDisplay;

        [SerializeField]
        public GameObject removeButton;

        [SerializeField]
        public bool isEquippedSlot;

        public GameObject item { get; private set; }

        private InputManager _inputManager;
        private bool isHighlighted;

        private void Awake()
        {
            _inputManager = new InputManager();
        }

        private void Update()
        {
            if ((EventSystem.current.currentSelectedGameObject == iconDisplay || isHighlighted) && _inputManager.UI.DropItem.triggered)
                DropItem(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHighlighted = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHighlighted = false;
        }

        private void OnEnable()
        {
            _inputManager.UI.Enable();
        }

        private void OnDisable()
        {
            _inputManager.UI.Disable();
        }

        public void SetItem(Item obj, bool weapon = false)
        {
            ToggleItem(obj.gameObject, obj.icon);
            obj.IsInInventory = true;
            obj.RenderItem(false);

            if (obj.gameObject.CompareTag("Ranged"))
            {
                obj.gameObject.GetComponent<PlayerShoot>().OnWeaponUnequipped();
            }

            if (weapon && isEquippedSlot)
            {
                EquipmentSystem.Instance.SetCurrentWeapon(obj.gameObject);
                obj.RenderItem(true);

                GameObject equippedWeapon = EquipmentSystem.Instance._equippedWeapon;

                if (equippedWeapon.CompareTag("Ranged"))
                {
                    equippedWeapon.GetComponent<PlayerShoot>().OnWeaponEquipped();
                }
            }
        }

        public void DropItem(bool spawn = false)
        {
            if (item is null)
                return;

            item.GetComponent<Item>().RenderItem(false);
            if (isEquippedSlot)
                EquipmentSystem.Instance.DetachWeapon();

            if (item.gameObject.CompareTag("Ranged") && isEquippedSlot)
            {
                item.GetComponent<PlayerShoot>().OnWeaponUnequipped();
            }

            if (spawn)
                item.GetComponent<Item>().Spawn();
            ToggleItem(null, null);
        }

        private void ToggleItem(GameObject obj, Sprite sprite)
        {
            // Get the Image and Button components of the icon display and remove button
            var iconImage = iconDisplay.GetComponent<Image>();
            var iconButton = iconDisplay.GetComponent<Button>();
            var removeImage = removeButton.GetComponent<Image>();
            var removeBtn = removeButton.GetComponent<Button>();

            // Set the item and icon sprite
            item = obj;
            iconImage.sprite = sprite;

            // Enable or disable the icon display and remove button based on whether the slot is empty
            var isEmpty = IsEmpty();
            iconImage.enabled = !isEmpty;
            removeImage.enabled = !isEmpty;
            removeBtn.interactable = !isEmpty;
        }

        public bool IsEmpty()
        {
            return item is null || item.gameObject is null;
        }

        public void Use()
        {
            InventoryManager.instance.UseItem(this);
        }
    }
}
