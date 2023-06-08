using UnityEngine;

namespace Controllers.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.Player playerObject;

        [SerializeField]
        public float startHealth;

        [SerializeField]
        private float startDamage;

        private void Start()
        {
            playerObject.entity.health = startHealth;
            playerObject.entity.baseDamage = startDamage;
        }

        public ScriptableObjects.Player GetPlayerObject()
        {
            return playerObject;
        }
    }
}
