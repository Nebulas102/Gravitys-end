using Core;
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
        private DialogueManager dialogue;

        [SerializeField]
        private GameObject skipDialogueButton;
        
        public static Navigation instance;

        [HideInInspector]
        public bool StageGenComplete;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if(loader != null && loader.activeSelf && StageGenComplete)
            {
                loader.SetActive(false);
                skipDialogueButton.SetActive(true);
            }
                
            if (loader != null && !loader.activeSelf && !dialogue.dialogueActive)
            {
                skipDialogueButton.SetActive(false);
                FadeIn();
            }
        } 

        public void MainMenu()
        {
            if(Timer.instance != null)
                Timer.instance.timerIsRunning = false;

            StartCoroutine(FadeOutCoroutine(0));
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
            StartCoroutine(FadeOutCoroutine(3));
        }

        public void GameOver()
        {
            Timer.instance.timerIsRunning = false;
            FadeOutCoroutine(4);
        }

        public void Quit()
        {
            StartCoroutine(QuitGame());
        }

        public void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        public IEnumerator FadeInCoroutine()
        {
            if(loader != null)
                loader.SetActive(false);

            if (dialogue != null)
                dialogue.SkipDialogue();

            if (loadingScreen != null)
            {
                animator.SetTrigger("FadeIn");
                yield return new WaitForSeconds(1f);
                loadingScreen.SetActive(false);
                yield return null;
            }
        }

        public IEnumerator FadeOutCoroutine(int scene)
        {
            if(loadingScreen != null)
            {
                loadingScreen.SetActive(true);
                animator.SetTrigger("FadeOut");
                yield return new WaitForSeconds(0.99f);
                SceneManager.LoadScene(scene);
                yield return null;
            }
            else
                SceneManager.LoadScene(scene);
        }

        public IEnumerator QuitGame()
        {
            loadingScreen.SetActive(true);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(0.99f);
            Application.Quit();
        }
    }
}