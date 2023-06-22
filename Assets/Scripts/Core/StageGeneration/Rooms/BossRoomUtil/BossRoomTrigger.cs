using System.Linq;
using Cinemachine;
using Core.Audio;
using Core.StageGeneration.Rooms.RoomTypes;
using Core.StageGeneration.Stage;
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
        private BossHallway bossHallway;

        private CinemachineVirtualCamera topDownCamera;

        private void Start()
        {
            bossRoom = transform.root.gameObject;
            player = PlayerManager.Instance.player;

            bossFightCanvas.enabled = false;

            bossHallway = FindObjectOfType<BossHallway>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var _bossRoom = bossRoom.GetComponent<BossRoom>();

            if (other.CompareTag("Player"))
            {
                _bossRoom.SetPlayerEnterBossFight(true);

                TeleportPlayer();

                ReplaceDoor(_bossRoom.GetDoors()[0]);
                bossHallway.endDoor.GetComponent<DoorModel>().Close();

                bossFightCanvas.enabled = true;

                gameObject.SetActive(false);

                // Play the boss music
                BackgroundMusicManager.SwitchToBossBackgroundMusic();
            }
        }

        private void ReplaceDoor(GameObject door)
        {
            RoomEditor.DoorBlock lDoorBlock = door.GetComponent<RoomEditor.DoorBlock>();
            if (lDoorBlock != null)
                lDoorBlock.CloseDoor();
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
