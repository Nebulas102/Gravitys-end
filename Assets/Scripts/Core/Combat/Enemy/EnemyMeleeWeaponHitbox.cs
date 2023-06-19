using Controllers.Player;
using UI.Tokens;
using UnityEngine;

public class EnemyMeleeWeaponHitbox : MonoBehaviour
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
        if (other.CompareTag("Player") && allowAttack)
        {
            other.GetComponent<PlayerStatsController>().TakeDamage(startDamage, endDamage, TokenManager.instance.healthSection.GetModifier());
        }
    }
}
