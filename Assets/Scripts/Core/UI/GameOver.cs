using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
