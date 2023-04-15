using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers.Enemy
{
    public class EnemyStatsController : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.Entity entity;
        [SerializeField]
        private float startHealth;
        [SerializeField]
        private float startDamage;

        private void Start()
        {
            entity.SetBaseHealth(startHealth);
            entity.SetBaseDamage(startDamage);
        }

        public ScriptableObjects.Entity GetEntity()
        {
            return entity;
        }
    }
}
