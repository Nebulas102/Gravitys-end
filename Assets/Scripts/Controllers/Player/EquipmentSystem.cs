using System.Collections;
using System.Collections.Generic;
using Core.UI.Inventory;
using UnityEngine;

namespace Controllers.Player
{
    public class EquipmentSystem : MonoBehaviour
    {
        public static EquipmentSystem Instance;


        [SerializeField] GameObject weaponHolder;
        [SerializeField] GameObject sheathHolder;


        private GameObject _equippedSword;

        GameObject currentWeaponInHand;
        GameObject currentWeaponInSheath;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        void Start()
        {
            var player = GameObject.FindWithTag("Player");
            _equippedSword = InventorySlot.FindWeapon(player.transform);
        }


        public void DrawWeapon()
        {
            Debug.Log("DrawWeapon");
            SetWeaponHolder(weaponHolder.transform);
        }

        public void SheathWeapon()
        {
            Debug.Log("SheathWeapon");
            SetWeaponHolder(sheathHolder.transform);
        }

        private void SetWeaponHolder(Transform holder)
        {
            _equippedSword.transform.SetParent(holder);
            _equippedSword.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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
