
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
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }

}