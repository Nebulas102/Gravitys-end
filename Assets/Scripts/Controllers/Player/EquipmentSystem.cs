using UnityEngine;

namespace Controllers.Player
{
    public class EquipmentSystem : MonoBehaviour
    {
        public static EquipmentSystem Instance;

        public GameObject currentWeaponInHand;

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
            currentWeaponInHand.name = "CurrentWeaponInHand";

            currentWeaponInHand.SetActive(false);
        }

        private void SetWeaponHolder(Transform holder)
        {
            if (currentWeaponInHand != null)
            {
                currentWeaponInHand.SetActive(true);
                currentWeaponInHand.transform.SetParent(holder);
                currentWeaponInHand.transform.SetLocalPositionAndRotation(Vector3.zero, currentWeaponInHand.transform.rotation);
            }
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
