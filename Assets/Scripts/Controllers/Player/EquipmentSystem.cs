using UnityEngine;

namespace Controllers.Player
{
    public class EquipmentSystem : MonoBehaviour
    {
        public static EquipmentSystem Instance;

        [SerializeField]
        public GameObject currentWeaponInHand;

        [SerializeField]
        private Transform weaponHolder;

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
            currentWeaponInHand = weapon;

            //Debug purposes
            if (weapon is not null)
                currentWeaponInHand.name = "CurrentWeaponInHand";

            SetWeaponHolder();
        }

        private void SetWeaponHolder()
        {
            if (currentWeaponInHand is null) return;

            currentWeaponInHand.transform.SetParent(weaponHolder);
            currentWeaponInHand.transform.SetLocalPositionAndRotation(Vector3.zero, currentWeaponInHand.transform.rotation);
        }

        public void DetachWeapon()
        {
            if (currentWeaponInHand is null) return;

            currentWeaponInHand.transform.SetParent(null);
            currentWeaponInHand = null;
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
