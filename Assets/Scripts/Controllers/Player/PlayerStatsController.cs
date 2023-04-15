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
        private float health;
        [SerializeField]
        private float damage;

        private void Start()
        {
            playerObject.entity.SetBaseHealth(health);
            playerObject.entity.SetBaseDamage(damage);
        }

        public ScriptableObjects.Player GetPlayerObject()
        {
            return playerObject;
        }
    }
}