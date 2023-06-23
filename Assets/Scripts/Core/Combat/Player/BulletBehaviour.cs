using Core.Enemy;
using UI.Tokens;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private int _minDamage;
    private int _maxDamage;
    private float _speed;
    private ParticleSystem _destructionEffect;
    private Vector3 _direction;

    private void Start()
    {
        _destructionEffect = Instantiate(_destructionEffect);
    }

    private void Update()
    {
        transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
        _destructionEffect.gameObject.transform.position = transform.root.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyBase>())
            {
                float damageMod = TokenManager.instance.damageSection.GetModifier();
                other.gameObject.GetComponent<EnemyBase>().TakeDamage((int)Mathf.Round(_minDamage * damageMod), (int)Mathf.Round(_maxDamage * damageMod), 0);
            }

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            if (other.gameObject.GetComponent<Boss>())
            {
                float damageMod = TokenManager.instance.damageSection.GetModifier();
                other.gameObject.GetComponent<Boss>().TakeDamage((int)Mathf.Round(_minDamage * damageMod), (int)Mathf.Round(_maxDamage * damageMod), 0);
            }

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
        {
            _destructionEffect.Play();
            Destroy(gameObject);
        }
    }

    public void SetDamage(int minDamage, int maxDamage)
    {
        _minDamage = minDamage;
        _maxDamage = maxDamage;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    public void SetDestructionEffect(ParticleSystem destructionEffect)
    {
        _destructionEffect = destructionEffect;
    }
}