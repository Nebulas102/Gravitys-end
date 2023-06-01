using UI.Runtime;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
    public class Entity : ScriptableObject
    {
        public float baseDamage;
        public Canvas canvas;
        public GameObject damageCounter;
        public float health;

        public void TakeDamage(int startDamage, int endDamage, float modifier)
        {
            var damage = Random.Range(startDamage, endDamage);
            // Subtract the armor value
            damage -= Mathf.RoundToInt(modifier / 100) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (damageCounter is not null)
            {
                // _damageCounter.GetComponent<DamageDisplay>().Show(damage.ToString(), this);
            }

            // Damage character
            health -= damage;

            if (health <= 0)
                Die();
        }

        public void Die()
        {
            //Load the death scene
            GameOver.Instance.PlayerGameOver();
        }
    }
}
