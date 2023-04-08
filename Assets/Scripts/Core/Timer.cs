using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Timer : MonoBehaviour
    {
        public float time = 1800;
        public float dangerZone = 600;
        public bool timerIsRunning = false;
        public Text display;

        private void Start()
        {
            // Starts the timer
            timerIsRunning = true;
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
                    Debug.Log("Time has run out!");
                    timerIsRunning = false;
                }
            }
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
