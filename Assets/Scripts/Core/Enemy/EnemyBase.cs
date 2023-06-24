using System.Collections;
using Controllers;
using UI;
using UI.Enemy;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        public float health;
        public Material hitMaterial;

        [Tooltip("Is in milliseconds (60f = 1 second)")]
        public float timeOnDead = 60f;

        public GameObject damageDisplay;

        private Canvas _canvas;

        private float _currentHealth;
        private ParticleSystem _hitParticle;

        public static int enemyKillCounter;

        public delegate void EnemyKilledEventHandler(EnemyBase enemy);

        public static event EnemyKilledEventHandler OnEnemyKilled;

        private void Start()
        {
            _canvas = GetComponentInChildren<Canvas>();
            _hitParticle = GetComponent<EnemyController>().hitParticle;
            _currentHealth = health;
        }

        private void Update()
        {
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
                GameStats.Instance.enemiesKilled++;
                if (OnEnemyKilled != null)
                {
                    OnEnemyKilled(this);
                }
            }
        }

        public void TakeDamage(int takeStartDamage, int takeEndDamage, float modifier)
        {
            var damage = Random.Range(takeStartDamage, takeEndDamage);
            damage -= Mathf.RoundToInt(modifier) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (damageDisplay != null)
                damageDisplay.GetComponent<DamageDisplay>().Show(damage.ToString(), damageDisplay, _canvas);

            _currentHealth -= damage;

            if (!_hitParticle.isPlaying)
            {
                _hitParticle.Play();
            }
        }

        private IEnumerator HitFeedback()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
