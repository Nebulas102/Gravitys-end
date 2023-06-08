using UnityEngine;
using TMPro;
using Core.Enemy;
using UI.Runtime;
using UI.Tokens;

namespace Core
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        public float time = 1800;

        [SerializeField]
        public float dangerZone = 600;

        [SerializeField]
        public TextMeshProUGUI display;

        private bool timerIsRunning;

        private void Start()
        {
            StartTimer();

            EnemyBase.OnEnemyKilled += AddEnemyTime;
        }

        private void Update()
        {
            // Checks whether the timer update should be executed
            if (timerIsRunning)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    time = 0;
                    timerIsRunning = false;
                    GameOver.Instance.PlayerGameOver();
                }
                DisplayTime(time);
            }
        }

        private void StartTimer()
        {
            // Starts the timer
            timerIsRunning = true;
        }

        private void AddEnemyTime(EnemyBase enemy)
        {
            time += enemy.timeOnDead * (TokenManager.instance.timeSection.GetModifier() + 1);
        }

        private void DisplayTime(float ttd)
        {
            float minutes = Mathf.FloorToInt(ttd / 60);
            float seconds = Mathf.FloorToInt(ttd % 60);
            float milliseconds = Mathf.FloorToInt((ttd * 1000) % 1000);

            if (ttd <= dangerZone)
            {
                display.color = Color.red;
            }

            display.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}
