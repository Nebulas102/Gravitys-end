using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Core
{
    public class MainMenu : MonoBehaviour
    {
        public void GoToCreditsMenu()
        {
            SceneManager.LoadScene("CreditsMenu");
        }
    }
}
