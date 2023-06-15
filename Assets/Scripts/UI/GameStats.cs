using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{  
    public static GameStats Instance;

    public int objectivesCompleted { get; set; }
    public int enemiesKilled { get; set; }
    public float timePlayed { get; set; }
    public float timeLeft { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
