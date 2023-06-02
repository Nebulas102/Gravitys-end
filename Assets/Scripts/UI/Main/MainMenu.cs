using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject loadingScreen;

        [SerializeField]
        private float minLoadingTime = 5f;

        private void Awake()
        {
            loadingScreen.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            var time = Time.time;
            var operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            operation.allowSceneActivation = false;

            loadingScreen.SetActive(true);
            mainMenu.SetActive(false);

            while (!operation.isDone)
            {
                var currentDuration = Time.time - time;
                if (currentDuration < minLoadingTime)
                    yield return new WaitForSeconds(minLoadingTime - currentDuration);

                operation.allowSceneActivation = true;
                yield return null;
            }
        }

        public void GoToCreditsMenu()
        {
            SceneManager.LoadScene("CreditsMenu");
        }

        public void GoToSettingsMenu()
        {
            SceneManager.LoadScene("SettingsMenu");
        }
    }
}
