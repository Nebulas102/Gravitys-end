
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Runtime
{
    public class GameOver : MonoBehaviour
    {
        public static GameOver Instance;

        private void Start()
        {
            if (Instance == null) Instance = this;
        }

        public void PlayerGameOver()
        {
            GameStats.Instance.timePlayed = Timer.Instance.startingTime - Timer.Instance.time;
            GameStats.Instance.timeLeft = Timer.Instance.time;
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }

}