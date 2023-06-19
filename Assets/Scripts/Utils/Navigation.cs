using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Navigation : MonoBehaviour
    {
        [Header("Loading (Async only)")]
        [SerializeField]
        private GameObject loadingScreen;

        [SerializeField]
        [Range(0f, 10f)]
        private float minLoadingTime = 5f;

        public void MainMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            Time.timeScale = 1f;
        }

        public void Credits()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void Settings()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        public void Game()
        {
            StartCoroutine(LoadSceneAsync(3));
        }

        public void GameOver()
        {
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private IEnumerator LoadSceneAsync(int index)
        {
            // Record the current time.
            var time = Time.time;

            // Load the scene asynchronously.
            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            // Don't allow the scene to activate until ready.
            operation.allowSceneActivation = false;
            loadingScreen.SetActive(true);

            // Wait until the scene is loaded.
            while (!operation.isDone)
            {
                // If it hasn't reached the minimum loading time yet, wait for it.
                var elapsedTime = Time.time - time;
                if (elapsedTime < minLoadingTime)
                    yield return new WaitForSeconds(minLoadingTime - elapsedTime);

                // Allow the scene to activate.
                operation.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}