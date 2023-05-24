using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Core
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] float time = 1800;
        [SerializeField] float dangerZone = 600;
        
        [SerializeField] TextMeshProUGUI display;

        bool timerIsRunning = false;

        private void Start()
        {
            StartTimer();
        }

        void Update()
        {
            // Checks whether the timer update should be executed
            if (timerIsRunning)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                    DisplayTime(time);
                }
                else
                {
                    time = 0;
                    DisplayTime(time);
                    timerIsRunning = false;
                    GameOver.Instance.PlayerGameOver();
                }
            }
        }

        public void StartTimer()
        {
            // Starts the timer
            timerIsRunning = true;
        }

        void DisplayTime(float ttd)
        {
            float minutes = Mathf.FloorToInt(ttd / 60);
            float seconds = Mathf.FloorToInt(ttd % 60);

            if (ttd <= dangerZone)
            {
                display.color = Color.red;
            }

            display.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
