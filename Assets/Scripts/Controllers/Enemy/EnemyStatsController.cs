using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Enemy
{
    public class EnemyStatsController : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.Entity entity;
        [SerializeField]
        private float startHealth;
        [SerializeField]
        private float startDamage;
        [SerializeField]
        private GameObject damageDisplay;

        private void Start()
        {
            entity.SetBaseHealth(startHealth);
            entity.SetBaseDamage(startDamage);
            entity.SetDamageCounter(damageDisplay);
            entity.SetCanvas(gameObject.GetComponentInChildren<Canvas>());

            entity.GetCanvas().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }

        // private void Update()
        // {
        //     if (Input.GetKey(KeyCode.G))
        //     {
        //         entity.TakeDamage(20f, 0f);
        //     }

        //     if (entity.GetHealth() <= 0)
        //     {
        //         Destroy(gameObject);
        //     }
        // }

        public ScriptableObjects.Entity GetEntity()
        {
            return entity;
        }
    }
}
