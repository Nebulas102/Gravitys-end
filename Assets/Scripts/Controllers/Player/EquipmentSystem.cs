using System;
using UnityEngine;

namespace Controllers.Player
{
    public class EquipmentSystem : MonoBehaviour
    {
        public static EquipmentSystem Instance;

        [SerializeField]
        public GameObject _equippedWeapon;

        [SerializeField]
        public GameObject _equippedArmor;

        [SerializeField]
        private Transform meleeHolder;

        [SerializeField]
        private Transform rangedHolder;

        private Quaternion oldRotation;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        void Start()
        {
            var player = GameObject.FindWithTag("Player");
        }

        public void SetCurrentWeapon(GameObject weapon)
        {
            _equippedWeapon = weapon;

            // This does nothing apparently
            if (_equippedWeapon is null)
            {
                PlayerAnimator.Instance._animator.SetTrigger("unequip");
            }

            if (_equippedWeapon.CompareTag("Melee"))
            {
                PlayerAnimator.Instance._animator.SetTrigger("meleeEquip");
                SetMeleeHolder();
            }

            if (_equippedWeapon.CompareTag("Ranged"))
            {
                PlayerAnimator.Instance._animator.SetTrigger("rangedEquip");
                SetRangedHolder();
            }

            // if (_equippedArmor.CompareTag("Armor")){
            //     SetArmorHolder();
            // }

                return;
        }

        private void SetArmorHolder()
        {
            if (_equippedArmor is null) return;

            _equippedArmor.transform.SetParent(rangedHolder);

            _equippedArmor.transform.localPosition = Vector3.zero;
            _equippedArmor.transform.localRotation = Quaternion.identity;
            _equippedArmor.transform.localScale = Vector3.one;

        }

        private void SetRangedHolder()
        {
            if (_equippedWeapon is null) return;

            _equippedWeapon.transform.SetParent(rangedHolder);

            _equippedWeapon.transform.localPosition = Vector3.zero;
            _equippedWeapon.transform.localRotation = Quaternion.identity;
            _equippedWeapon.transform.localScale = Vector3.one;

        }

        private void SetMeleeHolder()
        {
            if (_equippedWeapon is null) return;

            oldRotation = _equippedWeapon.transform.rotation;

            _equippedWeapon.transform.SetParent(meleeHolder);

            _equippedWeapon.transform.localPosition = Vector3.zero;
            _equippedWeapon.transform.localRotation = Quaternion.identity;
            _equippedWeapon.transform.localScale = Vector3.one;
        }

        public void DetachWeapon()
        {
            if (_equippedWeapon == null) return;

            _equippedWeapon.transform.rotation = oldRotation;
            PlayerAnimator.Instance._animator.SetTrigger("unequip");
            _equippedWeapon.transform.SetParent(null);
            _equippedWeapon = null;
        }
    }
}
