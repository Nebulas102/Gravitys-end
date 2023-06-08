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
        private Transform weaponHolder;

        [SerializeField]
        private Transform gunHolder;

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

            if (_equippedWeapon.CompareTag("Melee")){
                SetWeaponHolder();
            }
            else if (_equippedWeapon.CompareTag("Gun")){
                SetGunHolder();
            }
        }

        private void SetGunHolder()
        {
            // if (_equippedWeapon is null) return;

            // _equippedWeapon.transform.SetParent(gunHolder);
            // // _equippedWeapon.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            // _equippedWeapon.transform.localPosition = Vector3.zero;
            // _equippedWeapon.transform.localRotation = Quaternion.identity;
        }

        private void SetWeaponHolder()
        {
            if (_equippedWeapon is null) return;
            
            oldRotation = _equippedWeapon.transform.rotation;
            
            _equippedWeapon.transform.SetParent(weaponHolder);

            _equippedWeapon.transform.localPosition = Vector3.zero;
            _equippedWeapon.transform.localRotation = Quaternion.identity;
        }

        public void DetachWeapon()
        {
            if (_equippedWeapon is null) return;

            _equippedWeapon.transform.rotation = oldRotation;
            
            _equippedWeapon.transform.SetParent(null);

            _equippedWeapon = null;
        }


        // public void StartDealDamage()
        // {
        //     currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
        // }
        // public void EndDealDamage()
        // {
        //     currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
        // }
    }
}
