
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
            GameStats.Instance.timePlayed = Timer.instance.startingTime - Timer.instance.time;
            GameStats.Instance.timeLeft = Timer.instance.time;
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }

}