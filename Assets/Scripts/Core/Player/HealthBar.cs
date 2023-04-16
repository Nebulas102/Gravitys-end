using Controllers.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player
{
    public class HealthBar : MonoBehaviour
    {
        private GameObject _player;
        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            //Cant get player on start because this UI element runs earlier than the spawnroom where the player is
            // player = PlayerManager.instance.player;

            // slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
            // slider.value = slider.maxValue;
        }

        private void Update()
        {
            if (_player is null)
            {
                _player = PlayerManager.instance.player;

                _slider.maxValue = _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
                _slider.value = _slider.maxValue;
            }
            else
                _slider.value = _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
        }

        private void OnAwake()
        {
            _slider.maxValue = _player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
        }
    }
}
