using Cinemachine;
using Core.StageGeneration.Rooms.RoomTypes;
using UI;
using UnityEngine;

namespace Core.StageGeneration.Rooms.BossRoomUtil
{
    public class BossRoomTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject teleportDestination;

        [SerializeField]
        private Canvas bossFightCanvas;

        private GameObject bossRoom;
        private GameObject player;

        private CinemachineVirtualCamera topDownCamera;

        private void Start()
        {
            bossRoom = transform.root.gameObject;
            player = PlayerManager.Instance.player;

            bossFightCanvas.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            var _bossRoom = bossRoom.GetComponent<BossRoom>();

            if (other.tag == "Player")
            {
                _bossRoom.SetPlayerEnterBossFight(true);

                TeleportPlayer();

                _bossRoom.GetDoors()[0].SetActive(true);

                bossFightCanvas.enabled = true;

                gameObject.SetActive(false);

                // Play the boss music
                BackgroundMusicManager.SwitchToBossBackgroundMusic();
            }
        }

        //if teleport doesn't work, check at the project settings > Physics > Auto Sync Transforms is enabled.
        private void TeleportPlayer()
        {
            player.transform.position = teleportDestination.transform.position;

            topDownCamera = GameObject.Find("Cinemachine Camera").GetComponent<CinemachineVirtualCamera>();
            topDownCamera.m_Lens.OrthographicSize = 9f;
        }
    }
}
