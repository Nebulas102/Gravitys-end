using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.Player playerObject;
        [SerializeField]
        private float startHealth;
        [SerializeField]
        private float startDamage;

        private void Start()
        {
            playerObject.entity.SetBaseHealth(startHealth);
            playerObject.entity.SetBaseDamage(startDamage);
        }

        private void Update()
        {
            if (playerObject.entity.GetHealth() <= 0)
            {
                playerObject.entity.Die();
            }
        }

        public ScriptableObjects.Player GetPlayerObject()
        {
            return playerObject;
        }
    }
}