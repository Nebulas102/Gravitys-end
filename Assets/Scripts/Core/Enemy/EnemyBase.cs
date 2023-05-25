using UI.Enemy;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField]
        public string name;

        [SerializeField]
        public int startDamage;

        [SerializeField]
        public int endDamage;

        [SerializeField]
        public float health;

        [SerializeField]
        public GameObject damageDisplay;

        private Canvas _canvas;

        private float _currentHealth;

        public static int enemyKillCounter;

        public delegate void EnemyKilledEventHandler(EnemyBase enemy);
        public static event EnemyKilledEventHandler OnEnemyKilled;

        private void Start()
        {
            _canvas = GetComponentInChildren<Canvas>();

            _currentHealth = health;
        }

        private void Update()
        {
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
                if (OnEnemyKilled != null)
                {
                    OnEnemyKilled(this);
                }
            }
        }

        public int GetDamage()
        {
            return Random.Range(startDamage, endDamage);
        }

        public void TakeDamage(int takeStartDamage, int takeEndDamage, float modifier)
        {
            var damage = Random.Range(takeStartDamage, takeEndDamage);
            damage -= Mathf.RoundToInt(modifier / 100) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (damageDisplay != null)
                damageDisplay.GetComponent<DamageDisplay>().Show(damage.ToString(), damageDisplay, _canvas);

            _currentHealth -= damage;
        }
    }
}
