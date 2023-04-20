using System.Collections;
using System.Collections.Generic;
using Core.Enemy.StageBosses;
using TMPro;
using UI.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Enemy
{
    public class Boss : MonoBehaviour
    {
        [SerializeField]
        private float health;

        [SerializeField]
        private int startDamage;

        [SerializeField]
        private int endDamage;

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private TextMeshProUGUI bossNameBar;

        [SerializeField]
        private List<BossAbilityStage> bossAbilityStages;

        [SerializeField]
        public GameObject damageDisplay;

        private Canvas _canvas;

        private BossAbility _currentBossAbility;
        private float _currentHealth;
        private bool _startAbilitiesSequence;
        private bool _startFight;

        private void Start()
        {
            _currentHealth = health;

            healthBar.maxValue = health;
            healthBar.value = healthBar.maxValue;

            bossNameBar.text = name;

            _canvas = GetComponentInChildren<Canvas>();
        }

        private void Update()
        {
            //Test taking damage
            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     TakeDamage(0f);
            // }

            if (!_startFight || _startAbilitiesSequence) return;

            StartCoroutine(LoopBossAbilities());
            _startAbilitiesSequence = true;
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
                if (bossAbilityStages[currentAbilityIndex].GetAmountOfTimesUsed() !=
                    bossAbilityStages[currentAbilityIndex].GetAmountOfTimes()) continue;

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

        public void TakeDamage(float modifier)
        {
            var damage = Random.Range(startDamage, endDamage);
            damage -= Mathf.RoundToInt(modifier / 100) * damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            if (damageDisplay != null)
                damageDisplay.GetComponent<DamageDisplay>().Show(damage.ToString(), damageDisplay, _canvas);

            _currentHealth -= damage;
        }

        public bool GetStartFight()
        {
            return _startFight;
        }

        public void SetStartFight(bool startFight)
        {
            _startFight = startFight;
        }

        public BossAbility GetCurrentAbility()
        {
            return _currentBossAbility;
        }

        public List<BossAbilityStage> GetBossAbilityStages()
        {
            return bossAbilityStages;
        }

        public float GetHealth()
        {
            return _currentHealth;
        }
    }
}
