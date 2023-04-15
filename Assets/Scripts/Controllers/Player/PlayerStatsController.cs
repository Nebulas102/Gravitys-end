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

        public ScriptableObjects.Player GetPlayerObject()
        {
            return playerObject;
        }
    }
}