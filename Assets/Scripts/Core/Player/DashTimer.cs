using Controllers.Player;
using TMPro;
using UnityEngine;

namespace Core.Player
{
    public class DashTimer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timer;

        private float _cooldown;

        private GameObject _player;

        private void Start()
        {
            //Cant get player on start because this UI element runs earlier than the spawnroom where the player is
            // player = PlayerManager.instance.player;
        }

        private void Update()
        {
            if (_player == null)
            {
                _player = PlayerManager.Instance.player;

                _cooldown = _player.GetComponent<Dashing>().GetDashTimer();
            }
            else
            {
                var dashAvailable = _player.GetComponent<Dashing>().GetDashAvailable();

                if (dashAvailable)
                    timer.text = "Dash";
                else
                    timer.text = "Cooldown";
            }
        }
    }
}
