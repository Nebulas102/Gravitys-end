using System.Collections;
using System.Collections.Generic;
using Core.Enemy.StageBosses;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Enemy
{
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

        private BossAbility _currentBossAbility;
        private bool _startAbilitiesSequence;
        private bool _startFight;

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
            if (_startFight && !_startAbilitiesSequence)
            {
                StartCoroutine(LoopBossAbilities());

                _startAbilitiesSequence = true;
            }
        }

        private IEnumerator LoopBossAbilities()
        {
            var currentAbilityIndex = 0;

            while (true)
            {
                // Set the current ability
                _currentBossAbility = bossAbilityStages[currentAbilityIndex].GetBossAbility();

                //Use the current ability
                yield return StartCoroutine(_currentBossAbility.UseBossAbility());

                // Increment the number of times used for the current ability
                bossAbilityStages[currentAbilityIndex].IncrementAmountOfTimesUsed();

                // Check if we've used the current ability enough times
                if (bossAbilityStages[currentAbilityIndex].GetAmountOfTimesUsed() ==
                    bossAbilityStages[currentAbilityIndex].GetAmountOfTimes())
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
                }
            }
        }

        public bool GetStartFight()
        {
            return _startFight;
        }

        public void SetStartFight(bool _startFight)
        {
            this._startFight = _startFight;
        }

        public BossAbility GetCurrentAbility()
        {
            return _currentBossAbility;
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
}
