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

        [Tooltip("Is in milliseconds (60f = 1 second)")]
        public float timeOnDead = 60f;

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
            if (Input.GetKeyDown(KeyCode.G))
            {
                TakeDamage(1000, 2000, 0f);
            }

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
            damage -= Mathf.RoundToInt(modifier) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (damageDisplay != null)
                damageDisplay.GetComponent<DamageDisplay>().Show(damage.ToString(), damageDisplay, _canvas);

            _currentHealth -= damage;
        }
    }
}
