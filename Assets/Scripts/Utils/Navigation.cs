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
        private GameObject loader;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        [Range(0f, 10f)]
        private float minLoadingTime = 5f;
        
        public static Navigation instance;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
            StartCoroutine(FadeToMainScene());
        }

        public void GameOver()
        {
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public IEnumerator CloseLoadingScreen()
        { 
            loader.SetActive(false);
            animator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
            loadingScreen.SetActive(false);
            yield return null;
        }

        public IEnumerator FadeToMainScene()
        {
            loadingScreen.SetActive(true);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(3);
            yield return null;
        }

        private IEnumerator LoadSceneAsync(int index)
        {
            // Record the current time.
            var time = Time.time;

            //Start loading transition
            loadingScreen.SetActive(true);
            animator.SetTrigger("FadeOut");

            // Wait for the animation to end
            yield return new WaitForSeconds(1.5f);

            // Load the scene asynchronously.
            var operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            while (!operation.isDone)
                yield return null;

            SceneManager.SetActiveScene(SceneManager.GetSceneAt(index));

            while (Time.time - time > minLoadingTime)
                yield return null;

            SceneManager.UnloadSceneAsync(0);
        }
    }
}