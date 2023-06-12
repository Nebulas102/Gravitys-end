using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverStatsHolder;

    private TMP_Text gameOverStatsText;

    void Start()
    {
        gameOverStatsText = gameOverStatsHolder.GetComponent<TextMeshProUGUI>();

        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        string statsText = "";

        statsText += "Objectives Completed: " + GameStats.Instance.objectivesCompleted.ToString() + "\n";
        statsText += "Enemies killed: " + GameStats.Instance.enemiesKilled + "\n";
        statsText += "Time played: " + FormatTime(GameStats.Instance.timePlayed) + "\n";
        statsText += "Time left: " + FormatTime(GameStats.Instance.timeLeft);

        gameOverStatsText.text = statsText;
    }

    private String FormatTime(float time) {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
