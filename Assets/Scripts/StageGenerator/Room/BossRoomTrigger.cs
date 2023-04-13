using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject teleportDestination;

    [SerializeField]
    private Canvas bossFightCanvas;

    private GameObject bossRoom;
    private GameObject player;

    void Start()
    {
        bossRoom = transform.parent.gameObject;
        player = GameObject.FindWithTag("Player");

        bossFightCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        BossRoom _bossRoom = bossRoom.GetComponent<BossRoom>();

        if (other.tag == "Player")
        {
            _bossRoom.SetPlayerEnterBossFight(true);

            TeleportPlayer();

            _bossRoom.GetDoors()[0].SetActive(true);

            bossFightCanvas.enabled = true;

            gameObject.SetActive(false);
        }
    }

    //if teleport doesn't work, check at the project settings > Physics > Auto Sync Transforms is enabled.
    private void TeleportPlayer()
    {
        player.transform.position = teleportDestination.transform.position;
    }
}
