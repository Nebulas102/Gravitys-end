using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;

    private int startDamage;
    private int endDamage;
    private PlayerManager playerManager;
    private Vector3 direction;

    private void Start()
    {
        playerManager = PlayerManager.Instance;

        direction = transform.up;
    }

    private void Update()
    {
        // transform.rotation = Quaternion.LookRotation(transform.forward, direction);

        transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBase>())
            {
                other.gameObject.GetComponent<EnemyBase>().TakeDamage(startDamage, endDamage, 0);
            }

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
    }

    public void SetDamage(int _startDamage, int _endDamage)
    {
        startDamage = _startDamage;
        endDamage = _endDamage;
    }
}
