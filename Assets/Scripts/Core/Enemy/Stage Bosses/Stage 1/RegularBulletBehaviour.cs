using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Player;
using ScriptableObjects;
using UnityEngine;

public class RegularBulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private float bulletDamage = 10f;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    private GameObject boss;
    private GameObject player;

    private void Start()
    {
        boss = BossManager.instance.boss;
        player = PlayerManager.instance.player;

        startPosition = transform.position;
        targetPosition = player.transform.position;

        transform.LookAt(new Vector3(targetPosition.x, startPosition.y, targetPosition.z));
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Entity playerEntity = player.GetComponent<PlayerStatsController>().GetPlayerObject().entity;

            playerEntity.TakeDamage(bulletDamage, 0.2f);

            Destroy(gameObject);
        } 
        
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
    }
}
