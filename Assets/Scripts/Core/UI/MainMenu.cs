using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject loadingScreen;

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
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingScreen.SetActive(true);
            mainMenu.SetActive(false);

            while (!operation.isDone) yield return null;
        }

        public void GoToCreditsMenu()
        {
            SceneManager.LoadScene("CreditsMenu");
        }
    }
}
