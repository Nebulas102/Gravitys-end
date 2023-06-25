using System.Collections;
using Core.Enemy;
using UI.Tokens;
using UnityEngine;

public class PlasmaBulletBehavior : BulletBehavior
{
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBase>())
            {
                float damageMod = TokenManager.instance.damageSection.GetModifier();
                other.gameObject.GetComponent<EnemyBase>().TakeDamage((int)Mathf.Round(_minDamage * damageMod), (int)Mathf.Round(_maxDamage * damageMod), 0);
            }

            Destroy(transform.root.gameObject);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            if (other.gameObject.GetComponent<Boss>())
            {
                float damageMod = TokenManager.instance.damageSection.GetModifier();
                other.gameObject.GetComponent<Boss>().TakeDamage((int)Mathf.Round(_minDamage * damageMod), (int)Mathf.Round(_maxDamage * damageMod), 0);
            }

            Destroy(transform.root.gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
        {
            StartCoroutine(DestroyBullet());
        }
    }

    private IEnumerator plasmaExplosion()
    {
        yield return null;
    }
}