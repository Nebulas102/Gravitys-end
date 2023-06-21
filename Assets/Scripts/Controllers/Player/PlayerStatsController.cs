using UI.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField]
        private float baseDamage;

        [SerializeField]
        private float health;

        private Slider _slider;

        private void Start()
        {
            _slider = GameObject.Find("Healthbar").GetComponent<Slider>();
            _slider.maxValue = health;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
                TakeDamage(10000, 10001, 0);

            _slider.value = health;
        }

        public void TakeDamage(int startDamage, int endDamage, float modifier)
        {
            var damage = Random.Range(startDamage, endDamage);
            // Subtract the armor value
            damage -= Mathf.RoundToInt(modifier) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // Damage character
            health -= damage;

            if (health <= 0)
                Die();
        }

        public void HealPlayer(float healPlayerAmount)
        {
            health += healPlayerAmount;
        }

        private void Die()
        {
            // Load the death scene
            GameOver.Instance.PlayerGameOver();
        }
    }
}
