using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject player;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        //Cant get player on start because this UI element runs earlier than the spawnroom where the player is
        // player = PlayerManager.instance.player;

        // slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
        // slider.value = slider.maxValue;
    }

    private void Update() 
    {
        if (player == null)
        {
            player = PlayerManager.instance.player;

            slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
            slider.value = slider.maxValue;
        }
        else
        {
            slider.value = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
        }
    }
}
