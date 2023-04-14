using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using UnityEngine.UI;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private Entity entity;

    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI bossNameBar;

    [SerializeField]
    private List<BossAbilityStage> bossAbilityStages;

    private BossAbility currentBossAbility;
    private bool startFight = false;
    private bool startAbilitiesSequence = false;

    private void Start()
    {
        entity.SetBaseHealth(health);
        entity.SetBaseDamage(damage);

        healthBar.maxValue = health;
        healthBar.value = healthBar.maxValue;

        bossNameBar.text = entity.name;
    }

    private void Update()
    {
        if(startFight && !startAbilitiesSequence)
        {
            StartCoroutine(LoopBossAbilities());

            startAbilitiesSequence = true;
        }
    }

    private IEnumerator LoopBossAbilities()
    {
        int currentAbilityIndex = 0;

        while(true)
        {
            // Set the current ability
            currentBossAbility = bossAbilityStages[currentAbilityIndex].GetBossAbility();

            //Use the current ability
            StartCoroutine(currentBossAbility.UseBossAbility());

            // Increment the number of times used for the current ability
            bossAbilityStages[currentAbilityIndex].IncrementAmountOfTimesUsed();

            // Check if we've used the current ability enough times
            if (bossAbilityStages[currentAbilityIndex].GetAmountOfTimesUsed() >= bossAbilityStages[currentAbilityIndex].GetAmountOfTimes())
            {
                // Reset the times used for the current ability
                bossAbilityStages[currentAbilityIndex].SetAmountOfTimesUsed(0);

                // Move to the next ability
                currentAbilityIndex++;
                if (currentAbilityIndex >= bossAbilityStages.Count)
                {
                    // We've reached the end of the abilities, so cycle back to the first ability
                    currentAbilityIndex = 0;
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public bool GetStartFight()
    {
        return startFight;
    }

    public void SetStartFight(bool _startFight)
    {
        startFight = _startFight;
    }

    public BossAbility GetCurrentAbility()
    {
        return currentBossAbility;
    }

    public List<BossAbilityStage> GetBossAbilityStages()
    {
        return bossAbilityStages;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetHealth()
    {
        return health;
    }
}
