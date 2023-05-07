using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;

    private float damage;

    private void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBase>())
            {
                other.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
