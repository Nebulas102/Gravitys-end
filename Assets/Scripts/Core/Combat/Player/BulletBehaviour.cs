using System.Collections;
using Core.Enemy;
using UI.Tokens;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private ParticleSystem destructionEffect;

    private int _minDamage;
    private int _maxDamage;
    private float _speed;
    private Vector3 _direction;

    private float destructiomTime;
    private bool allowMovement = true;

    private void Update()
    {
        if (allowMovement)
        {
            transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
            destructionEffect.gameObject.transform.position = transform.root.position;
        }
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

    private IEnumerator DestroyBullet()
    {
        allowMovement = false;

        destructionEffect.Play();

        gameObject.GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(destructionEffect.main.duration);

        Destroy(transform.root.gameObject);
    }

    public void SetBulletStyle(Color albedo, Color glow, float glowPower, Gradient trailGradient)
    {
        EffectsHelper.ChangeShaderColorMaterial(transform, "_Albedo", albedo);
        EffectsHelper.ChangeShaderColorMaterial(transform, "_Glow", glow);

        EffectsHelper.ChangeShaderFloatMaterial(transform, "_GlowPower", glowPower);

        trail.colorGradient = trailGradient;
    }

    public void SetBulletDestructionStyle(Color standard, Color emission, Color nonEmissive)
    {
        EffectsHelper.ChangeShaderColorParticle(destructionEffect.transform, "_BaseColor", standard);
        EffectsHelper.ChangeShaderColorParticle(destructionEffect.transform, "_EmissionColor", emission);

        EffectsHelper.ChangeShaderColorParticle(destructionEffect.transform, "HitFeedbackExtra", "_BaseColor", nonEmissive);

        EffectsHelper.ChangeShaderColorParticle(destructionEffect.transform, "HitFeedbackParticleTrail", "_BaseColor", standard);
        EffectsHelper.ChangeShaderColorParticle(destructionEffect.transform, "HitFeedbackParticleTrail", "_EmissionColor", emission);

        EffectsHelper.ChangeShaderColorTrailParticle(destructionEffect.transform, "HitFeedbackParticleTrail", "_BaseColor", standard);
        EffectsHelper.ChangeShaderColorTrailParticle(destructionEffect.transform, "HitFeedbackParticleTrail", "_EmissionColor", emission);
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
}