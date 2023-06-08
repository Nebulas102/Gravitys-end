using Controllers.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.health;
        }

        private void Start()
        {
            //Cant get player on start because this UI element runs earlier than the spawn room where the player is
            // player = PlayerManager.instance.player;

            // slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.GetHealth();
            // slider.value = slider.maxValue;
        }

        private void Update()
        {
            if (player is null)
            {
                player = PlayerManager.Instance.player;

                _slider.maxValue = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.health;
                _slider.value = _slider.maxValue;
            }
            else
                _slider.value = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.health;
        }
    }
}
