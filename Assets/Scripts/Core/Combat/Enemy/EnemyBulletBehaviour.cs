using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;

    private int startDamage;
    private int endDamage;
    private Vector3 direction;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
    }

    private void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(startDamage, endDamage, 0);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
    }

    public void SetDamage(int _startDamage, int _endDamage)
    {
        startDamage = _startDamage;
        endDamage = _endDamage;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }
}
