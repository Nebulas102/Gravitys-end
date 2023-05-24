using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity")]
    public class Entity : ScriptableObject
    {
        public float baseDamage;
        public Canvas canvas;
        public GameObject damageCounter;
        public float health;

        public void TakeDamage(float damage, float modifier)
        {
            // Subtract the armor value
            damage -= modifier / 100 * damage;
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
