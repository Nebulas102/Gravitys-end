using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
    public class Entity : ScriptableObject
    {
        private float _baseHealth;
        private float _baseDamage;
        public string Name;

        public void SetBaseHealth(float health)
        {
            _baseHealth = health;
        }

        public float GetHealth()
        {
            return _baseHealth;
        }

        public void SetBaseDamage(float damage)
        {
            _baseDamage = damage;
        }

        public float GetDamage()
        {
            return _baseDamage;
        }
    }
}