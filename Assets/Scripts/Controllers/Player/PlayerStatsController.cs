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

        [HideInInspector]
        public int hitCounter;

        private Slider _slider;
        private Character _player;
        private ParticleSystem _hitParticle;

        private void Start()
        {
            _slider = GameObject.Find("Healthbar").GetComponent<Slider>();
            _slider.maxValue = health;

            _player = transform.GetComponent<Character>();
            _hitParticle = _player.hitParticle;
        }

        private void Update()
        {
            _slider.value = health;
        }

        public void TakeDamage(int startDamage, int endDamage, float modifier)
        {
            if (health <= 0)
                Die();

            var damage = Random.Range(startDamage, endDamage);
            // Subtract the armor value
            damage -= Mathf.RoundToInt(modifier) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            // Damage character
            health -= damage;
            
            if (!_hitParticle.isPlaying)
            {
                _hitParticle.Play();
            }
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
