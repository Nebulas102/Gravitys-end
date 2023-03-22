using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class MainMenu : MonoBehaviour
    {
        public void GoToCreditsMenu()
        {
            SceneManager.LoadScene("CreditsMenu");
        }
    }
}
