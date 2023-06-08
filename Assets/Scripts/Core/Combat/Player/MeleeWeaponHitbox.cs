using Core.Enemy;
using UI.Tokens;
using UnityEngine;

public class MeleeWeaponHitbox : MonoBehaviour
{
    private int startDamage;
    private int endDamage;

    [HideInInspector]
    public bool allowAttack;

    public void SetDamageHitbox(int _startDamage, int _endDamage)
    {
        startDamage = _startDamage;
        endDamage = _endDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        float damageMod = TokenManager.instance.damageSection.GetModifier() + 1;
        int baseDamage = (int)Mathf.Round(startDamage * damageMod);
        int maxDamage = (int)Mathf.Round(endDamage * damageMod);

        if (other.CompareTag("Enemy") && allowAttack)
        {
            other.GetComponent<EnemyBase>().TakeDamage(baseDamage, maxDamage, 0);
        }

        if (other.CompareTag("Boss") && allowAttack)
        {
            other.GetComponent<Boss>().TakeDamage(baseDamage, maxDamage, 0);
        }
    }
}
