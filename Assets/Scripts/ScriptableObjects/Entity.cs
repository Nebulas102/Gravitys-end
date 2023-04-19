using UI.Damage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
    public class Entity : ScriptableObject
    {
        private float _health;
        private float _baseDamage;
        
        private GameObject _damageCounter;
        private Canvas _canvas;

        public void SetBaseHealth(float health)
        {
            _health = health;
            if(_health <= 0)
            {
                Die();
            }
        }

        public float GetHealth()
        {
            return _health;
        }

        public void SetBaseDamage(float damage)
        {
            _baseDamage = damage;
        }

        public void TakeDamage(float damage, float modifier)
        {
            // Substract the armor value
            damage -= (modifier / 100) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (_damageCounter != null)
            {
                // _damageCounter.GetComponent<DamageDisplay>().Show(damage.ToString(), this);
            }

            // Damage character
            _health -= damage;

            Debug.Log(_health);
        }

        public float GetDamage()
        {
            return _baseDamage;
        }

        public GameObject GetDamageCounter()
        {
            return _damageCounter;
        }

        public void SetDamageCounter(GameObject damageCounter)
        {
            _damageCounter = damageCounter;
        }

        public Canvas GetCanvas()
        {
            return _canvas;
        }

        public void SetCanvas(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Die()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}