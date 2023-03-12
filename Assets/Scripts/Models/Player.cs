using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Entity _entity;

        [SerializeField]
        private Gear _armor;

        [SerializeField]
        private Gear _weapon;

        private void Start()
        {
            _entity.SetBaseHealth(100);
            _entity.SetBaseDamage(10);
            _entity.Name = "Player";
        }

        public void SwitchArmor(Gear armor)
        {
            _armor = armor;
        }

        public void SwitchWeapon(Gear weapon)
        {
            _weapon = weapon;
        }
    }
}