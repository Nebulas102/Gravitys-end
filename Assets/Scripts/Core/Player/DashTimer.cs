using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using UnityEngine;
using TMPro;

public class DashTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer;

    private GameObject player;
    private float cooldown;

    private void Start()
    {
        //Cant get player on start because this UI element runs earlier than the spawnroom where the player is
        // player = PlayerManager.instance.player;
    }

    private void Update()
    {
        if (player == null)
        {
            player = PlayerManager.instance.player;

            cooldown = player.GetComponent<Dashing>().GetDashTimer();
        }
        else
        {

            bool dashAvailable = player.GetComponent<Dashing>().GetDashAvailable();

            if (dashAvailable)
            {
                timer.text = "Dash";
            }
            else
            {
                timer.text = "Cooldown";
            }
        }        
    }
}
