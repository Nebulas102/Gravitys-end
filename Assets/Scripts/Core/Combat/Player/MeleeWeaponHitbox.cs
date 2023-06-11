using Core.Enemy;
using UI.Tokens;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    private int minDamage;
    private int maxDamage;

    [HideInInspector]
    public bool allowAttack = false;

    public void SetDamageHitbox(int _minDamage, int _maxDamage)
    {
        minDamage = _minDamage;
        maxDamage = _maxDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        float damageMod = TokenManager.instance.damageSection.GetModifier();
        int baseDamage = (int)Mathf.Round(minDamage * damageMod);
        int maxBaseDamage = (int)Mathf.Round(maxDamage * damageMod);

        if (other.CompareTag("Enemy") && allowAttack)
        {
            other.GetComponent<EnemyBase>().TakeDamage(baseDamage, maxBaseDamage, 0);
        }

        if (other.CompareTag("Boss") && allowAttack)
        {
            other.GetComponent<Boss>().TakeDamage(baseDamage, maxBaseDamage, 0);
        }
    }
}
